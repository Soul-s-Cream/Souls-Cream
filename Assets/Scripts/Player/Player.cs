using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Photon.PunBehaviour
{
    #region Public Field
    [Header("Movements")]

    public float moveSpeed;
    public float moveSmooth = .05f;
    public float jumpForce;
    public float maxSpeed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float velocityYMax = 50f;

    public Vector2 groundCheckingCenter;
    public float groundCheckingRadius;
    public LayerMask groundLayer;

    [Header("Screams")]
    public LayerMask humidityLayer;
    [Tooltip("Radius of the curiosity scream revealling effect")]
    public float curiosityScreamRadius;
    [Tooltip("Layer of objects to reveal by using the curiosity scream")]
    public LayerMask curiosityScreamLayer;
    [Tooltip("Instantiate when the player use the sadness scream")]
    public GameObject humidityPrefab;
    public float sadnessScreamMinGroundDistance = 1f;
    public Vector2 sadnessGroundCheckingCenter;

    public ScreamType selectedScream;

    [Header("Tags (used to get few specifics objects)")]
    [Tooltip("Tag of white player")]
    [TagSelector]
    public string whitePlayerTag;
    [Tooltip("Tag of black player")]
    [TagSelector]
    public string blackPlayerTag;
    [Tooltip("The tag of the wall impacted by the solitude scream")]
    [TagSelector]
    public string solitudeScreamReceiversTag;

    public enum ScreamType
    {
        Cornered,
        Envy,
        Solitude,
        Sadness,
        Compassion,
        Curiosity,
        Pride,
        Joy
    }

    public Role role;

    [Header("Cinématique")]
    public bool chuteLibre = false;
    public bool freezeMove = false;
    #endregion

    #region Private Fields
    private int jumpAmount = 0;
    private bool isJuming;
    private bool isGrounded;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 scale;
    private bool isScreaming;
    private SpriteRenderer spriteRender;

    private Vector3 velocity = Vector3.zero;
    // d�placement
    private float horizontalMovement = 0f;

    private Controls control;

    private float currentSadnessScreamRadius;

    private bool dernierCriActive = false;
    #endregion

    private void Awake()
    {
        control = new Controls();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        scale = transform.eulerAngles;
        //AkSoundEngine.PostEvent("MozTuto", gameObject);
    }

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            return;
        }

        transform.eulerAngles = scale;
        isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + groundCheckingCenter, groundCheckingRadius, groundLayer);
        horizontalMovement = control.Deplacement.Deplacement.ReadValue<float>() * moveSpeed;
        PlayerMove(horizontalMovement);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (control.Deplacement.Deplacement.ReadValue<float>() == 1)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, true);
            if (isGrounded)
            {
                //AkSoundEngine.PostEvent("MozFootsteps", gameObject);
            }
        }
        if (control.Deplacement.Deplacement.ReadValue<float>() == -1)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, false);
            if (isGrounded)
            {
                //AkSoundEngine.PostEvent("MozFootsteps", gameObject);
            }
        }
        if (isGrounded && control.Deplacement.Deplacement.ReadValue<float>() == 0)
        {
            //AkSoundEngine.PostEvent("MozLanding", gameObject);
        }
    }
    private void Update()
    {
        if (!photonView.isMine)
        {
            return;
        }

        if (velocity.y > velocityYMax)
        {
            velocity.y = velocityYMax;
        }
        if (control.Deplacement.Jump.triggered && jumpAmount < 1)
        {
            isJuming = true;
            jumpAmount += 1;
        }
        if (isGrounded)
        {
            jumpAmount = 0;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !control.Deplacement.Jump.triggered)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }
        else anim.SetBool("Jump", true);

        ScreamSelection();
        Screaming();
    }

    void PlayerMove(float _horizontalMovement)
    {
        if (isScreaming)
        {
            return;
        }

        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, moveSmooth);

        if (isJuming == true)
        {
            if (jumpAmount == 0)
            {
                AkSoundEngine.PostEvent("MozJump", gameObject);
                //AkSoundEngine.SetState("OzMozThemeAmbience", "MozBig");
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (jumpAmount == 1)
            {

                rb.AddForce(new Vector2(0f, jumpForce * 0f));
            }
            isJuming = false;
        }
    }

    [PunRPC]
    public void FlipToggleSprite(bool flipState)
    {
        GetComponent<SpriteRenderer>().flipX = flipState;
    }

    #region Scream
    public void ScreamSelection()
    {
        if (control.Cri.CriUp.triggered)
        {
            selectedScream = (ScreamType) ((int) (selectedScream + 1) % Enum.GetValues(typeof(ScreamType)).Length);
        }
        if (control.Cri.CriDown.triggered)
        {
            selectedScream = (ScreamType)((int)(selectedScream - 1) % Enum.GetValues(typeof(ScreamType)).Length);
        }
    }
    public void Screaming()
    {
        if (control.Cri.Cri.triggered && !isScreaming)
        {
            anim.SetTrigger(Enum.GetName(typeof(ScreamType), selectedScream));

            if (PhotonNetwork.connected)
            {
                switch (selectedScream)
                {
                    case ScreamType.Compassion: photonView.RPC("CompassionScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Cornered: photonView.RPC("CorneredScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Curiosity: photonView.RPC("CuriosityScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Envy: photonView.RPC("EnvyScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Joy: photonView.RPC("JoyScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Sadness: photonView.RPC("SadnessScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Pride: photonView.RPC("PrideScream", PhotonTargets.AllBufferedViaServer); break;
                    case ScreamType.Solitude: photonView.RPC("SolitudeScream", PhotonTargets.AllBufferedViaServer); break;
                }
            }
            else
            {
                switch (selectedScream)
                {
                    case ScreamType.Compassion: CompassionScream(); break;
                    case ScreamType.Cornered: CorneredScream(); break;
                    case ScreamType.Curiosity: CuriosityScream(); break;
                    case ScreamType.Envy: EnvyScream(); break;
                    case ScreamType.Joy: JoyScream(); break;
                    case ScreamType.Sadness: SadnessScream(); break;
                    case ScreamType.Pride: PrideScream(); break;
                    case ScreamType.Solitude: SolitudeScream(); break;
                }
            }

            StartCoroutine(WaitDuringScreaming());
        }
        else
        {
            anim.SetInteger("Scream", 0);
        }
    }

    IEnumerator WaitDuringScreaming()
    {
        isScreaming = true;
        yield return new WaitForSeconds(1);
        isScreaming = false;
    }
    #endregion

    #region screamsRPC
    [PunRPC]
    public void CompassionScream()
    {
        // endgame
    }

    [PunRPC]
    public void CorneredScream()
    {
        Player whitePlayer = GameObject.FindGameObjectWithTag(whitePlayerTag).GetComponent<Player>();
        Player blackPlayer = GameObject.FindGameObjectWithTag(blackPlayerTag).GetComponent<Player>();
        whitePlayer.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        blackPlayer.transform.localScale.Set(0.45f, 0.45f, 0.45f);
    }

    [PunRPC]
    public void CuriosityScream()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, curiosityScreamRadius, curiosityScreamLayer))
        {
            collider.GetComponent<CuriosityObject>().Reveal();
        }
    }

    [PunRPC]
    public void EnvyScream()
    {
        // endgame
    }

    [PunRPC]
    public void JoyScream()
    {
        jumpAmount = 2;
    }

    [PunRPC]
    public void PrideScream()
    {
        Player whitePlayer = GameObject.FindGameObjectWithTag(whitePlayerTag).GetComponent<Player>();
        Player blackPlayer = GameObject.FindGameObjectWithTag(blackPlayerTag).GetComponent<Player>();
        if (blackPlayer)
        {
            blackPlayer.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
        if (whitePlayer)
        {
            whitePlayer.transform.localScale.Set(0.45f, 0.45f, 0.45f);
        }
    }

    [PunRPC]
    public void SadnessScream()
    {
        if (PhotonNetwork.connected)
        {
            RaycastHit2D hit2D = Physics2D.Raycast((Vector2) transform.position + sadnessGroundCheckingCenter, Vector2.down, sadnessScreamMinGroundDistance, groundLayer);
            if (hit2D.collider != null)
            {
                if (PhotonNetwork.connected)
                {
                    PhotonNetwork.Instantiate(humidityPrefab.name, hit2D.point, Quaternion.identity, 0);
                }
                else
                {
                    Instantiate(humidityPrefab, hit2D.point, Quaternion.identity);
                }
            }
        }
    }

    [PunRPC]
    public void SolitudeScream()
    {
        foreach(GameObject platform in GameObject.FindGameObjectsWithTag(solitudeScreamReceiversTag))
        {
            platform.SetActive(false);
        }

        StartCoroutine(WaitForSolitudeScream());
    }

    private IEnumerator WaitForSolitudeScream()
    {
        yield return new WaitForSeconds(5);

        foreach (GameObject platform in GameObject.FindGameObjectsWithTag(solitudeScreamReceiversTag))
        {
            platform.SetActive(true);
        }
    }
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        control.Deplacement.Enable();
        control.Cri.Enable();
    }
    private void OnDisable()
    {
        control.Deplacement.Disable();
        control.Cri.Disable();
    }
    #endregion

    #region Cinématique
    /// <summary>
    /// s'active au cour de la dernière cinematique
    /// </summary>
    public void DernierCri()
    {
        dernierCriActive = true;
    }

    public void ChuteLibre()
    {
        freezeMove = true;
        chuteLibre = !chuteLibre;
        if (chuteLibre)
        {
            rb.gravityScale = 0;
            rb.velocity -= rb.velocity;
        }
        else
        {
            rb.gravityScale = 1;
            StartCoroutine(PosFin());
        }
    }
    private IEnumerator PosFin()
    {
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("positionFinalBool", true);

    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3) groundCheckingCenter, groundCheckingRadius);
        Gizmos.DrawLine(transform.position + (Vector3)sadnessGroundCheckingCenter, transform.position + Vector3.down * sadnessScreamMinGroundDistance);
    }
}