using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
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
    }
}
