using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Elevator)), CanEditMultipleObjects]
public class ElevatorGizmo : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    static void OnDrawDoorGizmo(Elevator elevator, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(elevator.endPoint, .1f);

        DrawArrow.ForGizmoTwoPoints(elevator.transform.position, elevator.endPoint, 0.25f, 20, 1f);
        DrawArrow.ForGizmoTwoPoints(elevator.endPoint, elevator.transform.position, 0.25f, 20, 1f);
    }

    protected virtual void OnSceneGUI()
    {
        Elevator elevator = (Elevator)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.PositionHandle(elevator.endPoint, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(elevator, "Change Look At Target Position");
            elevator.endPoint = newTargetPosition;
        }
    }
}