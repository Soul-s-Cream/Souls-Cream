using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EndLevel : MonoBehaviour
{
    private bool isPlayerReachEnd = false;
    private Animator animator;

    public bool IsPlayerReachEnd
    {
        get { return isPlayerReachEnd;  }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        AddListener();
    }

    void AddListener()
    {
        GameEvents.Instance.playerReachEnd += OnPlayerReachEnd;
    }

    void RemoveListener()
    {
        GameEvents.Instance.playerReachEnd -= OnPlayerReachEnd;
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    void OnPlayerReachEnd(GameObject gameObject)
    {
        EndLevel endPoint = gameObject.GetComponent<EndLevel>();

        if (gameObject != this.gameObject && this.isPlayerReachEnd)
        {
            animator.SetBool("EndLevel", true);
            endPoint.GetComponent<Animator>().SetBool("EndLevel", true);
            GameManager.Instance.EndLevel();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerReachEnd = true;
            GameEvents.Instance.TriggerPlayerReachEnd(this.gameObject);
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
