using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MecanismRelationship
{
    public static float sphereSelectionRadius = 0.15f;

    [DrawGizmo(GizmoType.Selected)]
    public static void OnDrawMecanismGizmo(Mecanism selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(selected.transform.position, sphereSelectionRadius);
        Gizmos.color = Color.yellow;
        Switch[] switches = Object.FindObjectsOfType<Switch>();
        foreach (Switch switchObj in switches)
        {
            foreach (Mecanism mecanism in switchObj.triggers)
            {
                if (mecanism == selected)
                    DrawArrow.ForGizmoTwoPoints(
                        OffsetPoint(switchObj.transform.position, selected.transform.position),
                        OffsetPoint(selected.transform.position, switchObj.transform.position),
                        0.25f, 20, 1f);
            }
        }
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    public static void OnDrawSwitchGizmo(Switch selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;

        #region Missing References Display
        int nbNullRef = 0;
        foreach (Mecanism mecanism in selected.triggers)
        {
            if (mecanism == null)
            {
                nbNullRef++;
                Debug.LogError("A Mecanism on " + selected.name + " is not defined! ", selected);
            }
        }

        if (nbNullRef != 0)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.fontSize = 20;
            Handles.Label(selected.transform.position + Vector3.down * 0.5f, nbNullRef + " Mecanism non référencés !?", style);
        }
        #endregion

        #region Mecanism Relationship Display
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            if (selected.triggers.Length != 0)
            {
                foreach (Mecanism mecanism in selected.triggers)
                {
                    if (mecanism != null)
                    {
                        DrawArrow.ForGizmoTwoPoints(
                        OffsetPoint(selected.transform.position, mecanism.transform.position),
                        OffsetPoint(mecanism.transform.position, selected.transform.position),
                        0.25f, 20, 1f);
                    }
                }
            }
        }
        #endregion
    }

    public static Vector3 OffsetPoint(Vector3 startPosition, Vector3 direction, float distance = 0.6f)
    {
        return startPosition + (direction - startPosition).normalized * distance;
    }
}

[CustomEditor(typeof(SwitchCondition))]
[CanEditMultipleObjects]
public class SwitchConditionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SwitchCondition sc = (SwitchCondition)target;
        base.OnInspectorGUI();
        sc.nbInputsRequirement = EditorGUILayout.IntSlider("Nb Input", sc.nbInputsRequirement, 2, 5);
    }
}
