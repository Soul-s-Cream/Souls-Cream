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
    [Tooltip("Prefabs à instancier pour le multijoueur")]
    public List<GameObject> PlayerPrefabs;
    public List<SpawnPoint> SpawnPointsList;
    public GameObject localPlayerInstance;
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
    }

    private void Start()
    {
        AddListener();
        InstantiatePlayer();
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

    public void GetSpawnPoints()
    {
        SpawnPointsList.Clear();
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (GameObject spawn in spawns)
        {
            SpawnPointsList.Add(spawn.GetComponent<SpawnPoint>());
        }
    }

    public void InstantiatePlayer()
    {
        GetSpawnPoints();
        if (PlayerPrefabs == null || PlayerPrefabs.Count == 0)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else if (SpawnPointsList == null)
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> SpawnPoint Reference. No spawning", this);
        }
        else if(localPlayerInstance == null)
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManager.GetActiveScene().name);
            string prefabName = "";
            SpawnPoint spawnPoint = SpawnPointsList[0];
            Role role = (Role)PhotonNetwork.player.CustomProperties["role"];
            string roleName = role == Role.BLANC ? "BLANC" : "NOIR";
            Debug.Log("Local player is " + roleName);

            LoadCameraRole(role);

            //On cherche l'avatar approprié pour le rôle du joueur
            foreach (GameObject prefab in PlayerPrefabs)
            {
                if(prefab.GetComponent<Player>().role == role)
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
            Debug.Log("Instantiating prefab " + prefabName + " for Player " + PhotonNetwork.player.ID);
            localPlayerInstance = PhotonNetwork.Instantiate(prefabName, spawnPoint.transform.position , Quaternion.identity, 0);
        }
    }

    public void LoadCameraRole(Role role)
    {
        Debug.Log("No camera set found");
        if(CameraSets._instance != null)
        {
            string roleName = role == Role.BLANC ? "BLANC" : "NOIR";
            Debug.Log("Loading camera for " + roleName);
            CameraSets._instance.DisplayCameraRole(role);
        }
    }

    public void EndLevel()
    {
        Debug.Log("Niveau terminé");
        NetworkManagerPUN.Instance.LoadScene(2);
    }

    #region Unity Callbacks

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        Debug.Log("On Scene Loaded");
        
        InstantiatePlayer();
    }

    #endregion
}
