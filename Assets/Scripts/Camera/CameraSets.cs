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
    //Ensemble de camera activ�es pour un r�le sp�cifique
    public List<CameraSetRole> cameraSetsRole;
    //Des cam�ras sp�ciales, notamment le tutoriel
    [SerializeField]
    public List<CameraSetSpecial> cameraSetSpecial;

    public static CameraSets _instance;

    public GameObject[] cameras;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        //On r�cup�re toutes les cam�ras
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
    }

    public void DisplayCameraRole(Role role)
    {
        List<Camera> cameraQueue = new List<Camera>();

        //On cherche l'ensemble de cam�ra pour le r�le demand�
        foreach (CameraSetRole camSetRol in cameraSetsRole)
        {
            if(camSetRol.role == role)
            {
                cameraQueue = new List<Camera>(camSetRol.cameras);
            }
        }
        //Pour chaque cam�ra dans la sc�ne, on regarde si elle appartient � l'ensemble demand�. Si oui, activ�e sinon d�sactiv�e.
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
