using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    BLANC,
    NOIR
}

public class GameManager : MonoBehaviour
{

    Controls controls;

    private static GameManager _instance;

    public Role role; 
    
    /// <summary>
    /// Return the actual instance of the GameManager
    /// </summary>
    public static  GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        //Singleton Patter
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        DontDestroyOnLoad(this);

        controls = new Controls();
    }

    private void Update()
    {
        if (controls.NetTest.ChangeScene.triggered)
            ChangeScene();
    }

    public void ChangeScene()
    {
        Debug.Log("Changing scene...");
        //CustomNetworkManager.singleton.ServerChangeScene("SampleNetworkScene2");
    }
}
