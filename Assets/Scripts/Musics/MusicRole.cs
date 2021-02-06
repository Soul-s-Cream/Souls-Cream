using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRole : MonoBehaviour
{
    [Tooltip("Le rôle que le joueur doit avoir pour entendre la musique")]
    public Role role;
    [Tooltip("L'événement posté pour lancer la musique")]
    public AK.Wwise.Event music;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.localPlayerInstance != null)
        {
            if(GameManager.Instance.localPlayerInstance.GetComponent<MovePlayerNet>() != null && GameManager.Instance.localPlayerInstance.GetComponent<MovePlayerNet>().role == role)
            {
                music.Post(gameObject);
            }
            else if(GameManager.Instance.localPlayerInstance.GetComponent<MovePlayer>() != null && GameManager.Instance.localPlayerInstance.GetComponent<MovePlayer>().role == role)
            {
                music.Post(gameObject);
            }
        }
    }

}
