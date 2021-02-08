using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FlipCamera : MonoBehaviour
{
    private Camera camera;
    private void Start()
    {
        camera = GetComponent<Camera>();
        Matrix4x4 mat = camera.projectionMatrix;
        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        camera.projectionMatrix = mat;
    }
}