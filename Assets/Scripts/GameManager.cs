using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Role
{
    BLANC,
    NOIR
}

public class GameManager : Photon.PunBehaviour
{

    Controls controls;

    private static GameManager _instance;

    [Tooltip("Le rôle de joueur du Client")]
    public Role role;

    public List<GameObject> PlayerPrefabs;
    public Queue<Role> RoleToDeal;
    public List<Transform> SpawnPointsList;

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
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        controls = new Controls();
        AddListener();
    }

    void AddListener()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void RemoveListener()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        RemoveListener();
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

    public void InstantiatePlayer()
    {
        if(PlayerPrefabs == null)
{
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManager.GetActiveScene().name);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.
            PhotonNetwork.Instantiate(this.PlayerPrefabs[PhotonNetwork.player.ID-1].name, SpawnPointsList[PhotonNetwork.player.ID-1].position, Quaternion.identity, 0);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SpawnPointsList.Clear();
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
        if (spawns[0].GetComponent<SpawnPoint>().role == Role.BLANC)
        {
            SpawnPointsList.Add(spawns[0].transform);
            SpawnPointsList.Add(spawns[1].transform);
        }
        else
        {
            SpawnPointsList.Add(spawns[1].transform);
            SpawnPointsList.Add(spawns[0].transform);
        }

        InstantiatePlayer();
    }
}
