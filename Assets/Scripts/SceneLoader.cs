using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Photon.PunBehaviour
{
    [Header("Scenes")]
    public ScreenFade screenFade;
    public int sceneId;

    public void Load()
    {
        if (screenFade)
        {
            StartCoroutine(WaitForFade());
        }
    }

    IEnumerator WaitForFade()
    {
        yield return StartCoroutine(screenFade.IFadeOut());
        LoadScene(sceneId);
    }

    public static void LoadScene(int id)
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.LoadLevel(id);
        }
        else
        {
            SceneManager.LoadScene(id, LoadSceneMode.Single);
        }
    }
}
