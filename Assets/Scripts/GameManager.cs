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
    #region Private Fields
    private static GameManager _instance;
    Controls controls;
    #endregion

    #region Public Fields
    [Tooltip("Prefabs � instancier pour le multijoueur")]
    public List<GameObject> PlayerPrefabs;
    public List<SpawnPoint> SpawnPointsList;
    #endregion

    /// <summary>
    /// Return the actual instance of the GameManager
    /// </summary>
    public static  GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        #region Singleton
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        #endregion

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
        else if (SpawnPointsList == null)
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> SpawnPoint Reference. No spawning", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManager.GetActiveScene().name);
            string prefabName = "";
            SpawnPoint spawnPoint = SpawnPointsList[0];
            Role role = (Role)PhotonNetwork.player.CustomProperties["role"];
            //On cherche l'avatar appropri� pour le r�le du joueur
            foreach (GameObject prefab in PlayerPrefabs)
            {
                if(prefab.GetComponent<MovePlayer>().role == role)
                {
                    prefabName = prefab.name;
                    break;
                }
            }
            //On cherche le point de Spawn appropri� pour l'avatar du joueur
            foreach(SpawnPoint spawn in SpawnPointsList)
            {
                if(spawn.role == role)
                {
                    spawnPoint = spawn;
                    break;
                }
            }

            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.
            PhotonNetwork.Instantiate(prefabName, spawnPoint.transform.position , Quaternion.identity, 0);
        }
    }

    public void EndLevel()
    {
        Debug.Log("Niveau termin�");
    }

    #region Unity Callbacks

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
