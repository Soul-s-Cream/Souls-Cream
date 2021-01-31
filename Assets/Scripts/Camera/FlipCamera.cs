using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlipCamera : MonoBehaviour
{

    public Camera m_camera;
    void OnPreCull()
    {
        m_camera.ResetWorldToCameraMatrix();
        m_camera.ResetProjectionMatrix();
        m_camera.projectionMatrix = m_camera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    void OnPreRender()
    {
        GL.invertCulling = true;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }

}