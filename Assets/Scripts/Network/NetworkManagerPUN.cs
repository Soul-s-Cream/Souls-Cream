using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManagerPUN : Photon.PunBehaviour
{
    #region Private Fields
    private static NetworkManagerPUN _instance;
    string gameVersion = "1";
    #endregion

    #region Public Fields
    public static NetworkManagerPUN Instance
    {
        get { return _instance; }
    }

    public bool offlineMode;

    public bool OfflineMode
    {
        set
        {
            offlineMode = value;
            PhotonNetwork.offlineMode = offlineMode;
        }
    }
    #endregion

    private void Awake()
    {
        //Singleton check
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        PhotonNetwork.automaticallySyncScene = true;
    }

    private void Start()
    {
        PhotonNetwork.offlineMode = offlineMode;

        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    public bool ConnectRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            return true;
        }

        return false;
    }
    public bool CreateRoom(string roomName)
    {
        if (PhotonNetwork.CreateRoom(roomName))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Fait charger une scène à tous les clients
    /// </summary>
    /// <param name="sceneName">Index de build la scène à charger</param>
    public void LoadScene(int sceneID)
    {
        PhotonNetwork.LoadLevel(sceneID);
    }

    /// <summary>
    /// Fait charger une scène à tous les clients
    /// </summary>
    /// <param name="sceneName">Nom de la scène à charger</param>
    public void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void SwitchRole()
    {
        Debug.Log("Switching Roles");
        Role role = (Role)PhotonNetwork.player.CustomProperties["role"];
        Hashtable paramCustom = new Hashtable
        {
            { "role", role == Role.BLANC ? Role.NOIR : Role.BLANC } 
        };

        PhotonNetwork.player.SetCustomProperties(paramCustom);
    }

    #region Photon.PunBehaviour Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon : Connected to Master.");
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("Photon : Disconnected from Photon.");
        StartCoroutine(MainReconnect());
    }

    private IEnumerator MainReconnect()
    {
        
        while (PhotonNetwork.networkingPeer.PeerState != ExitGames.Client.Photon.PeerStateValue.Disconnected)
        {
            Debug.Log("Waiting for client to be fully disconnected..", this);
            
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Client is disconnected!", this);

        if (!PhotonNetwork.ReconnectAndRejoin())
        {
            if (PhotonNetwork.Reconnect())
            {
                Debug.Log("Successful reconnected!", this);
            }
        }
        else
        {
            Debug.Log("Successful reconnected and joined!", this);
        }
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError(System.Enum.GetName(typeof(DisconnectCause), cause));
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon : Joined a room as Player " + PhotonNetwork.player.ID);

        Hashtable paramPlayer = new Hashtable
        {
            { "role", "0" }
        };

        if (PhotonNetwork.player.IsMasterClient)
        {
            paramPlayer["role"] = Role.BLANC;
        }
        else
        {
            Role roleMaster = (Role)PhotonNetwork.masterClient.CustomProperties["role"];
            paramPlayer["role"] = roleMaster == Role.BLANC ? Role.NOIR : Role.BLANC;
        }

        string roleNameDebug = "";
        switch (paramPlayer["role"])
        {
            case Role.BLANC:
                roleNameDebug = "BLANC";
                break;
            case Role.NOIR:
                roleNameDebug = "NOIR";
                break;
        }
        Debug.Log("Change role of Player " + PhotonNetwork.player.ID + " to " + roleNameDebug);

        PhotonNetwork.player.SetCustomProperties(paramPlayer);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Photon : player " + newPlayer.ID + " joined the room");
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer playerChanged = playerAndUpdatedProps[0] as PhotonPlayer;
        Hashtable properties = playerAndUpdatedProps[1] as Hashtable;

        //Si le joueur affecté n'est pas le joueur local
        if (playerChanged != PhotonNetwork.player)
        {
            if (properties.ContainsKey("role") && properties["role"] == PhotonNetwork.player.CustomProperties["role"])
            {
                Hashtable newProperties = new Hashtable();
                newProperties.Add("role", (Role)properties["role"] == Role.BLANC ? Role.NOIR : Role.BLANC);
                string roleNameDebug = "";
                switch (newProperties["role"])
                {
                    case Role.BLANC:
                        roleNameDebug = "BLANC";
                        break;
                    case Role.NOIR:
                        roleNameDebug = "NOIR";
                        break;
                }
                Debug.Log("Change role of Player " + PhotonNetwork.player.ID + " to " + roleNameDebug);
                PhotonNetwork.player.SetCustomProperties(newProperties);
            }
        }
    }

    #endregion

}
