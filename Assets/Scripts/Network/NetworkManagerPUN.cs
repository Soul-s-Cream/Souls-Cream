using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    #region Photon.PunBehaviour Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon : Connected to Master.");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("Photon : Disconnected from Photon.");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Photon : Failed to join a random room. Creating one...");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon : Joined a room as Player " + PhotonNetwork.player.ID);
        GameManager.Instance.InstantiatePlayer();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Photon : player "+ newPlayer.ID +" joined the room");
    }

    #endregion

}
