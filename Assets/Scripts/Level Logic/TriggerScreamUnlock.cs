using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TriggerScreamUnlock : MonoBehaviour
{
    [Tooltip("Cri à débloquer")]
    public ScreamType screamUnlocked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player.photonView.isMine)
        {
            player.UnlockScream(screamUnlocked);
            Destroy(this.gameObject);
        }
    }
}
