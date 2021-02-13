using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DoorGizmo : MonoBehaviour
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void OnDrawDoorGizmo(Door door, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        SpriteRenderer spriteRender = door.GetComponent<SpriteRenderer>();
        Vector3 destination = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);

        switch (door.openingDirection)
        {
            
            case Direction.HAUT:
                destination.y += spriteRender.bounds.size.y;
                break;
            case Direction.BAS:
                destination.y -= spriteRender.bounds.size.y;
                break;
            case Direction.GAUCHE:
                destination.x -= spriteRender.bounds.size.y; 
                break;
            case Direction.DROITE:
                destination.x += spriteRender.bounds.size.y; 
                break;
        }
        DrawArrow.ForGizmoTwoPoints(door.transform.position, destination, 0.25f, 20, 0.95f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.1f);
    }
}
