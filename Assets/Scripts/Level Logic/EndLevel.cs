using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EndLevel : MonoBehaviour
{
    [Tooltip("Radius of the portal")]
    public float radius;
    [Tooltip("Layer of players")]
    public LayerMask layer;
    [Tooltip("Optionnal tag(s) of player(s)")]
    public List<string> playerTags;
    [Tooltip("Scene to load")]
    public SceneReference sceneToLoad;

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
            GameManager.Instance.EndLevel(sceneToLoad.ScenePath);
        }

    }

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, layer);

        if (collider)
        {
            if (playerTags.Count > 0)
            {
                foreach (string tag in playerTags)
                {
                    if (collider.gameObject.CompareTag(tag))
                    {
                        isPlayerReachEnd = true;
                        GameEvents.Instance.TriggerPlayerReachEnd(this.gameObject);
                        return;
                    }
                }
            }
            else
            {
                isPlayerReachEnd = true;
                GameEvents.Instance.TriggerPlayerReachEnd(this.gameObject);
                return;
            }
        }
        else
        {
            isPlayerReachEnd = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
