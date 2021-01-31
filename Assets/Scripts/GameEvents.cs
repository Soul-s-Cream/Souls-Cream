using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents _instance;
    public static GameEvents Instance
    {
        get
        {
            return _instance;
        }
    }
    #region Définition des événements
    public delegate void CustomEventTypeExample(GameObject obj, string aString);
    public event CustomEventTypeExample customEventExample;

    public delegate void GameEvent(GameObject gameObject);
    public event GameEvent PlayerReachEnd;

    public event Action BasicEventExemple;
    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

    }
    
    public void LaunchPlayerReachEnd(GameObject gameObject)
    {
        PlayerReachEnd(gameObject);
    }
}
