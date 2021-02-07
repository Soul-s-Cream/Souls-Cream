using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class OfflinePlayButton : Photon.PunBehaviour
{
    [Tooltip("On ne récupère que le blanc, car si le blanc est faux, alors le noir est forcément vrai")]
    public Toggle roleBlanc;

    private void Awake()
    {
        if(!Debug.isDebugBuild)
            Destroy(this.gameObject);
    }

    public void StartOfflineMode()
    {
        //Si le client est connecté, on le déconnecte
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();

        NetworkManagerPUN.Instance.OfflineMode = true;
        NetworkManagerPUN.Instance.CreateRoom("offline");
    }
}
