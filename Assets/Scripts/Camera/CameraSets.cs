using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraSetRole
{
    public Role role;
    public List<Camera> cameras;
}

[Serializable]
public class CameraSetSpecial
{
    public string nameSet;
    public List<Camera> cameras;
}

public class CameraSets : MonoBehaviour
{
    //Ensemble de camera activées pour un rôle spécifique
    public List<CameraSetRole> cameraSetsRole;
    //Des caméras spéciales, notamment le tutoriel
    [SerializeField]
    public List<CameraSetSpecial> cameraSetSpecial;

    public static CameraSets _instance;

    public GameObject[] cameras;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        //On récupère toutes les caméras
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
    }

    public void DisplayCameraRole(Role role)
    {
        List<Camera> cameraQueue = new List<Camera>();

        //On cherche l'ensemble de caméra pour le rôle demandé
        foreach (CameraSetRole camSetRol in cameraSetsRole)
        {
            if(camSetRol.role == role)
            {
                cameraQueue = new List<Camera>(camSetRol.cameras);
            }
        }
        //Pour chaque caméra dans la scène, on regarde si elle appartient à l'ensemble demandé. Si oui, activée sinon désactivée.
        foreach(GameObject camera in cameras)
        {
            Camera camComp = camera.GetComponent<Camera>();
            if (cameraQueue.Count > 0)
            {
                for(int i = 0; i < cameraQueue.Count; i++)
                {
                    if (cameraQueue[i] == camComp)
                    {
                        camComp.enabled = true;
                        cameraQueue.Remove(cameraQueue[i]);
                    }
                    else
                        camComp.enabled = false;
                }
            }
            else
            {
                camComp.enabled = false;
            }
        }
    }

    public void DisplayCameraSpecial(string cameraName)
    {
        throw new NotImplementedException();
    }
}
