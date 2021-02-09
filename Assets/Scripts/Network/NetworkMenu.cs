using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// S'occupe de gérer la logique du menu du jeu
/// </summary>
public class NetworkMenu : Photon.PunBehaviour
{
    [Header("Panels Menus")]
    public RectTransform titleMenuPanel;
    public RectTransform errorMessagePanel;
    public RectTransform successMessagePanel;
    public RectTransform connectionPanel;
    public RectTransform roomPanel;
    [Header("Texts")]
    public Text roomNameField;
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

    //La vue de menu actuellement affichée
    RectTransform viewDisplayed;

    #region Menu View Logic
    /// <summary>
    /// Permet d'afficher la vue en paramètre en masquant l'actuelle affichée
    /// </summary>
    /// <param name="viewToDisplay">La vue de menu à afficher</param>
    public void DisplayView(RectTransform viewToDisplay)
    {
        try
        {
            if(viewDisplayed != null)
                viewDisplayed.gameObject.SetActive(false);
            viewDisplayed = viewToDisplay;
            viewDisplayed.gameObject.SetActive(true);
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("A view is not referenced : " + e.Source.ToString());
        }
    }
    #endregion

    #region Title Menu Logic
    private void Start()
    {
        DisplayView(titleMenuPanel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Network Menu Logic
    public void LeaveRoom()
    {
        DisplayView(connectionPanel);

        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom()
    {
        if (NetworkManagerPUN.Instance.CreateRoom(roomNameField.text))
        {

        }
        else
        {
            DisplayView(errorMessagePanel);
        }
    }

    public void JoinRoom()
    {
        if (NetworkManagerPUN.Instance.ConnectRoom(roomNameField.text))
        {

        }
        else
        {
            DisplayView(errorMessagePanel);
        }
    }

    public void StartGame(int sceneID)
    {
        NetworkManagerPUN.Instance.LoadScene(sceneID);
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
        if(PhotonNetwork.offlineMode == true)
        {
            launchButton.interactable = true;
        }
        else if (PhotonNetwork.room.PlayerCount == 2 && player1ReadyToggle.isOn && player2ReadyToggle.isOn)
            launchButton.interactable = true;
    }

    private void UpdateAll()
    {
        UpdateConnections();
        UpdateRole();
        UpdatePlayButton();
    }
    #endregion

    #region Photon.PUNBehaviour Callbacks
    public override void OnJoinedRoom()
    {
        DisplayView(successMessagePanel);
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

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        UpdateAll();
    }

    public override void OnDisconnectedFromPhoton()
    {
        DisplayView(connectionPanel);
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
