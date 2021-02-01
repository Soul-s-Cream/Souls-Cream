using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NetworkMenu : Photon.PunBehaviour
{
    public Text roomNameField;
    public Text playerNameField;
    public RectTransform errorMessagePanel;
    public RectTransform successMessagePanel;
    public RectTransform connectionPanel;
    public RectTransform roomPanel;

    public Text roomNameText;
    public Text player1Name;
    public Toggle player1Toggle;
    public Text player2Name;
    public Toggle player2Toggle;

    public Button launchButton;

    public void loadScene(int levelId)
    {
        PhotonNetwork.LoadLevel(levelId);
    }

    public void LeaveRoom()
    {
        roomPanel.gameObject.SetActive(false);
        connectionPanel.gameObject.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom()
    {
        if (NetworkManagerPUN.Instance.CreateRoom(roomNameField.text))
        {
            
        }
        else
        {
            errorMessagePanel.gameObject.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        if (NetworkManagerPUN.Instance.ConnectRoom(roomNameField.text))
        {
            
        }
        else
        {
            errorMessagePanel.gameObject.SetActive(true);
        }
    }

    private void UpdateRoom()
    {
        if (PhotonNetwork.room.PlayerCount >= 1)
        {
            player1Name.text = PhotonNetwork.playerList[0].ID + " is connected";
            player1Toggle.isOn = true;
        }
        else
        {
            player1Name.text = "Not connected";
            player1Toggle.isOn = false;
        }

        if (PhotonNetwork.room.PlayerCount >= 2)
        {
            player2Name.text = PhotonNetwork.playerList[1].ID + " is connected";
            player2Toggle.isOn = true;
        }
        else
        {
            player2Name.text = "Not connected";
            player2Toggle.isOn = false;
        }

        launchButton.interactable = PhotonNetwork.room.PlayerCount >= 2;
    }

    public override void OnJoinedRoom()
    {
        successMessagePanel.gameObject.SetActive(true);
        connectionPanel.gameObject.SetActive(false);
        roomNameText.text = PhotonNetwork.room.Name;
        UpdateRoom();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        UpdateRoom();
    }

    public override void OnDisconnectedFromPhoton()
    {
        roomPanel.gameObject.SetActive(false);
        connectionPanel.gameObject.SetActive(true);
    }
}
