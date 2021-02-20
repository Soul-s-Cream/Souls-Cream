using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlateform)), CanEditMultipleObjects]
public class MovingPlateformGizmo : Editor
{
    /// <summary>
    /// Opacit� du cube de pr�visualisation
    /// </summary>
    private static float cubePreviewOpacity = 0.2f;
    /// <summary>
    /// D�calage des fl�ches de trajectoire vis � vis du centre de la trajectoire
    /// </summary>
    private static float offsetArrow = 0.1f;
    /// <summary>
    /// Distance de d�calage du texte "Destination"
    /// </summary>
    private static float offsetDestinationText = 0.3f;

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    static void OnDrawDoorGizmo(MovingPlateform movingPlateform, GizmoType gizmoType)
    {
        SpriteRenderer spriteRender = movingPlateform.GetComponent<SpriteRenderer>();
        
        #region Cube Preview Display
        //On d�finit la forme du cube de pr�visualisation, selon si le sprite est plus large que haut ou non
        Vector3 cubePreviewSize = new Vector3(0.4f * movingPlateform.transform.localScale.x, 1.71f * movingPlateform.transform.localScale.y, 0.5f);
        if (spriteRender.bounds.size.x > spriteRender.bounds.size.y)
            cubePreviewSize = new Vector3(1.4f * movingPlateform.transform.localScale.x, 0.5f * movingPlateform.transform.localScale.y, 0.5f);
        //On applique au cube la rotation de l'objet
        Matrix4x4 cubeTransform = movingPlateform.transform.localToWorldMatrix;
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
        Gizmos.matrix *= cubeTransform;
        //Couleur du cube de pr�visualisation
        Gizmos.color = Color.yellow * (Color.white - Color.black * cubePreviewOpacity);
        //On dessine le cube
        Gizmos.DrawCube(movingPlateform.transform.InverseTransformPoint(movingPlateform.endPosition), cubePreviewSize);
        Gizmos.matrix = oldGizmosMatrix;
        #endregion

        #region Arrow Trajectory Display
        //Couleur des fl�ches
        Gizmos.color = Color.red;
        //On calcul la direction d'un vecteur orthogonal � la direction du point de fin, qu'on �chellonne avec le d�calage des fl�ches
        Vector3 offsetDirection = Quaternion.Euler(0, 0, 90) * (movingPlateform.endPosition - movingPlateform.transform.position).normalized * offsetArrow;
        //On dessine les fl�ches en direction du point de fin de trajectoire
        DrawArrow.ForGizmoTwoPoints(movingPlateform.transform.position + offsetDirection,
            movingPlateform.endPosition + offsetDirection,
            0.25f, 20, 1f);
        DrawArrow.ForGizmoTwoPoints(movingPlateform.transform.position -offsetDirection,
            movingPlateform.endPosition - offsetDirection,
            0.25f, 20, 1f);
        //Si la plateforme fait des all�es retours entre les deux points, alors on le signale en dessinant des fl�ches qui vont de la fin au point de d�part
        if (movingPlateform.looping)
        {
            DrawArrow.ForGizmoTwoPoints(movingPlateform.endPosition + offsetDirection,
                movingPlateform.transform.position + offsetDirection,
                0.25f, 20, 1f);
            DrawArrow.ForGizmoTwoPoints(movingPlateform.endPosition - offsetDirection,
                movingPlateform.transform.position - offsetDirection,
                0.25f, 20, 1f);
        }
        #endregion

        #region Text Display
        //On affiche le texte "Destination"
        Handles.Label(movingPlateform.endPosition + (Vector3.up + Vector3.right) * offsetDestinationText + Vector3.forward * -5f, "Destination");
        //On affiche le texte de la dur�e total pour parcourir une trajectoire
        Vector3 middlePointDirection = (movingPlateform.endPosition - movingPlateform.transform.position) / 2;
        Handles.Label(
            movingPlateform.transform.position + middlePointDirection + Vector3.right * 0.3f + Vector3.forward * -5f,
            movingPlateform.TimeReachingPosition(movingPlateform.endPosition).ToString("0.00") + "s"
            );
        #endregion
    }

    /// <summary>
    /// Affiche les Handlers pour le point de destination
    /// </summary>
    protected virtual void OnSceneGUI()
    {
        MovingPlateform elevator = (MovingPlateform)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.PositionHandle(elevator.endPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(elevator, "Change Look At Target Position");
            elevator.endPosition = newTargetPosition;
        }
    }
}