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
    

    private static GameManager _instance;
    Controls controls;

    [Tooltip("Le r�le de joueur du Client")]
    public Role player1Role = Role.BLANC;
    public Role player2Role = Role.NOIR;

    public List<GameObject> PlayerPrefabs;
    public List<SpawnPoint> SpawnPointsList;

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

    #region Ajout des Listeners
    void AddListener()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void RemoveListener()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

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
            string prefabName = "";
            SpawnPoint spawnPoint = SpawnPointsList[0];

            switch(PhotonNetwork.player.ID)
            {
                case 1:
                    foreach(GameObject prefab in PlayerPrefabs)
                    {
                        if (player1Role == Role.BLANC && prefab.name == "PlayerWhiteNet")
                            prefabName = prefab.name;
                        else if (player1Role == Role.NOIR && prefab.name == "PlayerBlackNet")
                            prefabName = prefab.name;
                    }
                    foreach (SpawnPoint spawn in SpawnPointsList)
                    {
                        if (spawn.role == player1Role)
                            spawnPoint = spawn;
                    }
                    break;
                case 2:
                    foreach (GameObject prefab in PlayerPrefabs)
                    {
                        if (player2Role == Role.BLANC && prefab.name == "PlayerWhiteNet")
                            prefabName = prefab.name;
                        else if (player2Role == Role.NOIR && prefab.name == "PlayerBlackNet")
                            prefabName = prefab.name;
                    }
                    foreach (SpawnPoint spawn in SpawnPointsList)
                    {
                        if (spawn.role == player1Role)
                            spawnPoint = spawn;
                    }
                    break;
            }
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.
            PhotonNetwork.Instantiate(prefabName, spawnPoint.transform.position , Quaternion.identity, 0);
        }
    }

    [PunRPC]
    public void SwitchRole()
    {
        if(player1Role == Role.BLANC)
        {
            player1Role = Role.NOIR;
            player2Role = Role.BLANC;
        }
        else
        {
            player1Role = Role.BLANC;
            player2Role = Role.NOIR;
        }
    }

    public void SwitchRoleNet()
    {
        photonView.RPC("SwitchRole", PhotonTargets.All);
    }

    #region Callbacks
    public void EndLevel()
    {
        Debug.Log("Niveau termin�");
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SpawnPointsList.Clear();
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
        foreach(GameObject spawn in spawns)
        {
            SpawnPointsList.Add(spawn.GetComponent<SpawnPoint>());
        }

        InstantiatePlayer();
    }

    #endregion
}
