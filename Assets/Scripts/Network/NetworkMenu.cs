using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkMenu : Photon.PunBehaviour
{
    [Header("Panels Menus")]
    public Text roomNameField;
    public RectTransform errorMessagePanel;
    public RectTransform successMessagePanel;
    public RectTransform connectionPanel;
    public RectTransform roomPanel;
    [Header("Texts")]
    public Text roomNameText;
    [Header("Ready Toggles")]
    public Toggle player1ReadyToggle;
    public Toggle player2ReadyToggle;
    [Header("Flip Connect / Disconnect Display")]
    public Image player1Connect;
    public Image player1Disonnect;
    public Image player2Connect;
    public Image player2Disonnect;
    [Header("Role Display")]
    public Toggle player1RoleW;
    public Toggle player1RoleB;
    public Toggle player2RoleW;
    public Toggle player2RoleB;
    [Header("Buttons")]
    public Button launchButton;
    public Button switchButton;

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

    public void ToggleReady(int playerId)
    {
        if (playerId == PhotonNetwork.player.ID)
        {
            Hashtable customProps = new Hashtable
            {
                { "ready", playerId == 1 ? player1ReadyToggle.isOn : player2ReadyToggle.isOn}
            };
            PhotonNetwork.player.SetCustomProperties(customProps);
        }
    }

    public void SwitchRole()
    {
        NetworkManagerPUN.Instance.SwitchRole();
        if (PhotonNetwork.isMasterClient)
        {
            Hashtable customProps = new Hashtable
        {
            { "ready", false}
        };

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                player.SetCustomProperties(customProps);
            }
        }
    }

    public void UpdateRole()
    {
        Role role = (Role)PhotonNetwork.masterClient.CustomProperties["role"];
        switch (role)
        {
            case Role.BLANC:
                player1RoleB.gameObject.SetActive(false);
                player1RoleW.gameObject.SetActive(true);
                player2RoleB.gameObject.SetActive(true);
                player2RoleW.gameObject.SetActive(false);
                break;
            case Role.NOIR:
                player1RoleB.gameObject.SetActive(true);
                player1RoleW.gameObject.SetActive(false);
                player2RoleB.gameObject.SetActive(false);
                player2RoleW.gameObject.SetActive(true);
                break;
        }
    }

    void UpdateConnections()
    {
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            Debug.Log("1 Player in Room");
            player1Connect.gameObject.SetActive(true);
            player1Disonnect.gameObject.SetActive(false);
            player2Connect.gameObject.SetActive(false);
            player2Disonnect.gameObject.SetActive(true);
            player1RoleB.isOn = true;
            player1RoleW.isOn = true;
        }
        else if (PhotonNetwork.room.PlayerCount == 2)
        {
            Debug.Log("2 Player in Room");
            player1Connect.gameObject.SetActive(true);
            player1Disonnect.gameObject.SetActive(false);
            player2Connect.gameObject.SetActive(false);
            player2Disonnect.gameObject.SetActive(true);
            player1RoleB.isOn = true;
            player1RoleW.isOn = true;
            player2RoleB.isOn = true;
            player2RoleW.isOn = true;
        }
        else
        {
            Debug.Log("No player in Room");
            player1Connect.gameObject.SetActive(false);
            player1Disonnect.gameObject.SetActive(true);
            player2Connect.gameObject.SetActive(false);
            player2Disonnect.gameObject.SetActive(true);
            player1RoleB.isOn = false;
            player1RoleW.isOn = false;
            player2RoleB.isOn = false;
            player2RoleW.isOn = false;

        }
    }

    void UpdatePlayButton()
    {
        if (PhotonNetwork.room.PlayerCount == 2 && player1ReadyToggle.isOn && player2ReadyToggle.isOn)
            launchButton.interactable = true;
    }

    private void UpdateAll()
    {
        UpdateConnections();
        UpdateRole();
        UpdatePlayButton();
    }

    #region Photon.PUNBehaviour Callbacks
    public override void OnJoinedRoom()
    {
        successMessagePanel.gameObject.SetActive(true);
        connectionPanel.gameObject.SetActive(false);
        roomNameText.text = PhotonNetwork.room.Name;

        switch (PhotonNetwork.player.ID)
        {
            case 1:
                player1ReadyToggle.interactable = true;
                break;
            case 2:
                player2ReadyToggle.interactable = true;
                break;
        }

        Hashtable customProps = new Hashtable
            {
                { "ready", false }
            };
        PhotonNetwork.player.SetCustomProperties(customProps);

        if (!PhotonNetwork.player.IsMasterClient)
        {
            //On désactive l'affichage pour Play si pas Master
            launchButton.gameObject.SetActive(false);
            switchButton.gameObject.SetActive(false);
        }

        UpdateAll();

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        UpdateAll();
    }

    public override void OnDisconnectedFromPhoton()
    {
        roomPanel.gameObject.SetActive(false);
        connectionPanel.gameObject.SetActive(true);
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer playerChanged = playerAndUpdatedProps[0] as PhotonPlayer;
        Hashtable properties = playerAndUpdatedProps[1] as Hashtable;

        //Si le joueur affecté n'est pas le joueur local
        if (playerChanged != PhotonNetwork.player)
        {
            if (properties.ContainsKey("ready"))
            {
                switch (playerChanged.ID)
                {
                    case 1:
                        player1ReadyToggle.isOn = (bool)properties["ready"];
                        break;
                    case 2:
                        player2ReadyToggle.isOn = (bool)properties["ready"];
                        break;
                }
            }
        }

        UpdateAll();
    }
    #endregion
}
