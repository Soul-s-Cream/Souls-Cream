using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EndLevel : MonoBehaviour
{
    private bool isPlayerReachEnd = false;

    public bool IsPlayerReachEnd
    {
        get { return isPlayerReachEnd;  }
    }

    private void Start()
    {
        AddListener();
    }

    void AddListener()
    {
        GameEvents.Instance.PlayerReachEnd += OnPlayerReachEnd;
    }

    void RemoveListener()
    {
        GameEvents.Instance.PlayerReachEnd -= OnPlayerReachEnd;
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    void OnPlayerReachEnd(GameObject gameObject)
    {
        if (gameObject != this.gameObject && this.isPlayerReachEnd)
            GameManager.Instance.EndLevel();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerReachEnd = true;
            GameEvents.Instance.LaunchPlayerReachEnd(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerReachEnd = false;
        }
    }
}
