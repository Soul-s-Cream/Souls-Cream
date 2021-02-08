using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MecanismRelationship
{
    [DrawGizmo(GizmoType.Selected)]
    public static void OnDrawMecanismGizmo(Mecanism selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        Switch[] switches = GameObject.FindObjectsOfType<Switch>();
        foreach (Switch switchObj in switches)
        {
            foreach (Mecanism mecanism in switchObj.triggers)
            {
                if (mecanism == selected)
                    Gizmos.DrawLine(selected.transform.position, switchObj.transform.position);
            }
        }
    }

    [DrawGizmo(GizmoType.Selected)]
    public static void OnDrawSwitchGizmo(Switch selected, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        foreach (Mecanism mecanism in selected.triggers)
        {
            Gizmos.DrawLine(selected.transform.position, mecanism.transform.position);
        }
    }
}
