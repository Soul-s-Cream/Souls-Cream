using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FlipCamera : MonoBehaviour
{
    public Camera camera;
    private void Start()
    {
        camera = GetComponent<Camera>();

        Matrix4x4 mat = camera.projectionMatrix;
        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
        camera.projectionMatrix = mat;
    }


    /*void OnPreCull()
    {
        Matrix4x4 scale;
        if (camera.aspect > 2)
        {
            scale = Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
        else
        {
            scale = Matrix4x4.Scale(new Vector3(1, -1, 1));
        }
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        camera.projectionMatrix = camera.projectionMatrix * scale;
    }
    void OnPreRender()
    {
        GL.invertCulling = true;
    }
    void OnPostRender()
    {
        GL.invertCulling = false;
    }*/
}