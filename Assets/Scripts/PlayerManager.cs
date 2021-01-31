using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.PunBehaviour
{
    public Role role;
    [Tooltip("L'instance locale du joueur. Utilisez ceci pour savoir si le joueur local est bien repr�sent� dans la sc�ne")]
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.isMine)
            PlayerManager.LocalPlayerInstance = this.gameObject;

        DontDestroyOnLoad(this.gameObject);
    }
}
