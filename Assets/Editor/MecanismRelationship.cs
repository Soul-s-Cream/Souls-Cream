using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MecanismRelationship
{
    [DrawGizmo(GizmoType.Selected)]
    public static void OnDrawMecanismGizmo(Mecanism selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(selected.transform.position, 0.3f);
        Gizmos.color = Color.yellow;
        Switch[] switches = GameObject.FindObjectsOfType<Switch>();
        foreach (Switch switchObj in switches)
        {
            foreach (Mecanism mecanism in switchObj.triggers)
            {
                if (mecanism == selected)
                    DrawArrow.ForGizmoTwoPoints(
                        IntersectionPointRadius(switchObj.transform.position, selected.transform.position),
                        IntersectionPointRadius(selected.transform.position, switchObj.transform.position),
                        0.25f, 20, 1f);
            }
        }
    }

    [DrawGizmo(GizmoType.Selected)]
    public static void OnDrawSwitchGizmo(Switch selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        if(selected.triggers.Length != 0)
        {
            foreach (Mecanism mecanism in selected.triggers)
            {
                DrawArrow.ForGizmoTwoPoints(
                    IntersectionPointRadius(selected.transform.position, mecanism.transform.position),
                    IntersectionPointRadius(mecanism.transform.position, selected.transform.position),
                    0.25f, 20, 1f);
            }
        }
    }

    public static Vector3 IntersectionPointRadius(Vector3 position, Vector3 direction, float radius = 0.6f)
    {
        return position + (direction - position).normalized * radius;
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
