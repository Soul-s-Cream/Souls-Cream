using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerPUN : Photon.PunBehaviour
{
    public List<GameObject> PrefabPlayers;
    public Queue<Role> RoleToDeal;

    #region Private Fields
    string gameVersion = "1";
    #endregion

    private void Awake()
    {
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
        Debug.Log("Photon : Joined a room.");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Photon : player "+ newPlayer.ID +" joined the room");
    }

    #endregion

}
