using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Photon.PunBehaviour
{
    [Header("Scenes")]
    public ScreenFade screenFade;
    public SceneReference scene;

    public void Load()
    {
        if (screenFade)
        {
            StartCoroutine(WaitForFade());
        }
        else
            LoadScene(scene);
    }

    IEnumerator WaitForFade()
    {
        yield return StartCoroutine(screenFade.IFadeOut());
        LoadScene(scene.ScenePath);
    }

    public static void LoadScene(string sceneName)
    {
        if (PhotonNetwork.connected)
        {
            NetworkManagerPUN.Instance.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
