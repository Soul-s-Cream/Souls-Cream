using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlateform)), CanEditMultipleObjects]
public class MovingPlateformGizmo : MecanismRelationship
{
    /// <summary>
    /// Opacité du cube de prévisualisation
    /// </summary>
    private static float cubePreviewTransparency = 0.80f;
    /// <summary>
    /// Décalage des flèches de trajectoire vis à vis du centre de la trajectoire
    /// </summary>
    private static float offsetArrow = 0.1f;
    /// <summary>
    /// Distance de décalage du texte "Destination"
    /// </summary>
    private static float offsetDestinationText = 0.3f;

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    static void OnDrawDoorGizmo(MovingPlateform movingPlateform, GizmoType gizmoType)
    {
        SpriteRenderer spriteRender = movingPlateform.GetComponent<SpriteRenderer>();
        BoxCollider2D collider2D = movingPlateform.GetComponent<BoxCollider2D>();
        
        #region Cube Preview Display
        //On définit la forme du cube de prévisualisation, selon si le sprite est plus large que haut ou non
        Vector3 cubePreviewSize = new Vector3(collider2D.size.x, collider2D.size.y, 0.5f);
        //On applique au cube la rotation de l'objet
        Matrix4x4 cubeTransform = movingPlateform.transform.localToWorldMatrix;
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
        Gizmos.matrix *= cubeTransform;
        //Couleur du cube de prévisualisation
        Gizmos.color = Color.yellow * (Color.white - Color.black * cubePreviewTransparency);
        //On dessine le cube
        Gizmos.DrawCube(
            movingPlateform.transform.InverseTransformPoint(movingPlateform.EndPosition) + Vector3.right * collider2D.offset.x + Vector3.up * collider2D.offset.y,
            cubePreviewSize);
        Gizmos.matrix = oldGizmosMatrix;
        #endregion

        #region Arrow Trajectory Display
        //Couleur des flèches
        Gizmos.color = Color.red;
        //On calcul la direction d'un vecteur orthogonal à la direction du point de fin, qu'on échellonne avec le décalage des flèches
        Vector3 offsetDirection = Quaternion.Euler(0, 0, 90) * (movingPlateform.EndPosition - movingPlateform.transform.position).normalized * offsetArrow;
        //On dessine les flèches en direction du point de fin de trajectoire
        DrawArrow.ForGizmoTwoPoints(movingPlateform.transform.position + offsetDirection,
            movingPlateform.EndPosition + offsetDirection,
            0.25f, 20, 1f);
        DrawArrow.ForGizmoTwoPoints(movingPlateform.transform.position - offsetDirection,
            movingPlateform.EndPosition - offsetDirection,
            0.25f, 20, 1f);
        //Si la plateforme fait des allées retours entre les deux points, alors on le signale en dessinant des flèches qui vont de la fin au point de départ
        if (movingPlateform.looping)
        {
            DrawArrow.ForGizmoTwoPoints(movingPlateform.EndPosition + offsetDirection,
                movingPlateform.transform.position + offsetDirection,
                0.25f, 20, 1f);
            DrawArrow.ForGizmoTwoPoints(movingPlateform.EndPosition - offsetDirection,
                movingPlateform.transform.position - offsetDirection,
                0.25f, 20, 1f);
        }
        #endregion

        #region Text Display
        //On affiche le texte de la durée total pour parcourir une trajectoire
        Vector3 middlePointDirection = (movingPlateform.EndPosition - movingPlateform.transform.position) / 2;
        Handles.Label(
            movingPlateform.transform.position + middlePointDirection + Vector3.right * 0.3f + Vector3.forward * -5f,
            movingPlateform.TimeReachingPosition(movingPlateform.EndPosition).ToString("0.00") + "s"
            );
        #endregion
    }

    /// <summary>
    /// Affiche les Handlers pour le point de destination
    /// </summary>
    protected virtual void OnSceneGUI()
    {
        MovingPlateform movingPlateform = (MovingPlateform)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.PositionHandle(movingPlateform.EndPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(movingPlateform, "Change Look At Target Position");
            movingPlateform.EndPosition = newTargetPosition;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        MovingPlateform movingPlateform = (MovingPlateform)target;

        EditorGUILayout.LabelField("Trajectory Duration :", movingPlateform.TimeReachingPosition(movingPlateform.EndPosition).ToString("0.00") + "s");


        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Reset End Point Position"))
        {
            movingPlateform.endPositionRelative = movingPlateform.transform.InverseTransformPoint(movingPlateform.DefaultEndPointPosition());
        }
        EditorUtility.SetDirty(movingPlateform);
    }
}