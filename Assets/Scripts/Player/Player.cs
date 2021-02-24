using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Photon.PunBehaviour
{
    #region Public Field
    public Role role;

    [Header("Movements")]

    public float moveSpeed;                       // Défini le moveSpeed du joueur
    public float moveSmooth = .05f;               // Mmmh
    public float jumpForce = 500;                 // valeur de la poussée vertical du saut
    public float DoubleJumpForce = 400;           // valeur de la force du double Jump
    public float deplacementEnAir = 1.01f;        // vitesse de déplacement du joueur lorsqu'il ne touche pas le sol          

    public float fallMultiplier = 2.5f;           // gère la vitesse de la chute. Permet un saut plus réaliste
    public float lowJumpMultiplier = 2f;          // gère la force ascendante lors du saut. Permet un saut plus réaliste
    public float velocityYMax = 50f;              // Permet de limiter la force du saut, notament lors du double saut

    public Vector2 groundCheckingCenter;
    public float groundCheckingRadius;
    public LayerMask groundLayer;

    public TagSelectorAttribute humidityTag;

    [Header("Screams")]
    public LayerMask humidityLayer;
    [Tooltip("Radius of the curiosity scream revealing effect")]
    public float curiosityScreamRadius;
    [Tooltip("Layer of objects to reveal by using the curiosity scream")]
    public LayerMask curiosityScreamLayer;
    [Tooltip("Instantiate when the player use the sadness scream")]
    public GameObject humidityPrefab;
    public float sadnessScreamMinGroundDistance = 1f;
    public Vector2 sadnessGroundCheckingCenter;
    [Tooltip("Index du cri selectionné dans la liste des cris appris")]
    public int screamUnlockedIndex = 0;
    [Tooltip("Cri selectionné")]
    public ScreamType selectedScream;
    [Tooltip("Informations concernant les cris")]
    public ScreamsData screamsData;
    //Données depuis le ScriptableObject ScreamsData mises dans un Dictionary pour faciliter le traitement
    public Dictionary<ScreamType, ScreamData> screamsDataParsed;

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
    [Header("Sound")]
    /// <summary>
    /// Liste des cris débloqués par le joueur
    /// </summary>
    public List<ScreamType> unlockedScreams;
    public AK.Wwise.Event playerMoveSound;
    public AK.Wwise.RTPC footStepsWetRTPC;
    #endregion

    #region Private Fields
    private int jumpAmount = 0;                     // nombre de saut en cours
    private bool isJuming;                          // si le joueur est en train de sauter
    private bool isGrounded;                        // vérifie si le joueur est en contacte avec le sol
    [HideInInspector]
    public Animator anim;
    private Rigidbody2D rb;
    private Vector3 scale;
    private bool isScreaming;
    private SpriteRenderer spriteRender;
    [HideInInspector]
    public CapsuleCollider2D capsuleCollider;
    [HideInInspector]
    public Vector2 defaultCapsuleColliderSize;
    private Vector2 defaultGroundCenter;
    private float defaultGroundRadius;

    private Vector3 velocity = Vector3.zero;
    // d�placement
    private float horizontalMovement = 0f;

    private Controls control;

    private float currentSadnessScreamRadius;

    private bool dernierCriActive = false;

    #endregion

    private void Awake()
    {
        //On initialise & récupère les différents composants nécessaires pour la logique de l'objet
        control = new Controls();
        spriteRender = GetComponent<SpriteRenderer>();
        unlockedScreams = new List<ScreamType>();

        #region Initialize Scream Data
        screamsDataParsed = new Dictionary<ScreamType, ScreamData>();
        foreach(ScreamData data in screamsData.data)
        {
            screamsDataParsed.Add(data.scream, data);
        }
        #endregion

        //On lie les contrôles
        control.Cri.CriUp.performed += ctx => ScreamSelection(true);
        control.Cri.CriDown.performed += ctx => ScreamSelection(false);
        control.Cri.Cri.performed += ctx => Screaming();

        AddListeners();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        defaultCapsuleColliderSize = capsuleCollider.size;
        defaultGroundCenter = groundCheckingCenter;
        defaultGroundRadius = groundCheckingRadius;
        scale = transform.eulerAngles;
        //AkSoundEngine.PostEvent("MozTuto", gameObject);
        AkSoundEngine.PostEvent("MozFootsteps", gameObject);
    }

    void FixedUpdate()
    {
        //Si le Player n'est pas contrôlé par le client local, alors on ignore les instructions dans FixedUpdate
        if (!photonView.isMine)
        {
            return;
        }

        transform.eulerAngles = scale;
        isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + groundCheckingCenter, groundCheckingRadius, groundLayer);
        horizontalMovement = control.Deplacement.Deplacement.ReadValue<float>() * moveSpeed;
        PlayerMove(horizontalMovement);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (control.Deplacement.Deplacement.ReadValue<float>() > 0)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, true);
            if (isGrounded)
            {
                //footStepsWetRTPC.SetGlobalValue(Mathf.Abs(rb.velocity.x));
            }
        }
        else if (control.Deplacement.Deplacement.ReadValue<float>() < 0)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, false);
            if (isGrounded)
            {
                AkSoundEngine.SetState("PlayerMoving", "Moving");
            }
        }
        else if (isGrounded && control.Deplacement.Deplacement.ReadValue<float>() == 0)
        {
            AkSoundEngine.SetState("PlayerMoving", "Stop");
        }
    }
    private void Update()
    {
        //Si le Player n'est pas contrôlé par le client local, alors on ignore les instructions dans Update
        if (!photonView.isMine)
        {
            return;
        }

        /*if (velocity.y > velocityYMax) // Permet de limiter la force du saut, notament lors du double saut
        {
            velocity.y = velocityYMax;
        }*/
        if (control.Deplacement.Jump.triggered && jumpAmount <= 1) //  si le joueur déclanche le saut
        {
            isJuming = true;
            jumpAmount += 1;
        }

        if (!isGrounded)  // gère la vitesse de la chute. Permet un saut plus réaliste
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !control.Deplacement.Jump.triggered)  // gère la force ascendante lors du saut. Permet un saut plus réaliste
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        if (isGrounded)                     // gère l'animation du saut
        {
            jumpAmount = 0;
            anim.SetBool("Jump", false);
        }
        else anim.SetBool("Jump", true);

    }

    #region Listeners
    /// <summary>
    /// Subscribe to all events required for the object
    /// </summary>
    private void AddListeners()
    {
        GameEvents.Instance.fireScreamAbility+= OnFireScreamAbility;
    }

    /// <summary>
    /// Unsubscribe to all events subscribed
    /// </summary>
    private void RemoveListeners()
    {
        GameEvents.Instance.fireScreamAbility -= OnFireScreamAbility;
    }
    #endregion

    #region Movement Logic
    void PlayerMove(float _horizontalMovement)
    {
        if (isScreaming)
        {
            return;
        }

        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        if (isGrounded)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, moveSmooth);        // gère le déplacement du joueur lorsqu'il touche le sol
        }
        else
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity * new Vector2(deplacementEnAir, 1f), targetVelocity, ref velocity, moveSmooth); // gère le déplacement horizontale du joueur lorsqu'il ne touche pas le sol
        }

        if (isJuming == true)   // premier saut
        {
            if (jumpAmount == 0)
            {
                AkSoundEngine.PostEvent("MozJump", gameObject);
                rb.AddForce(new Vector2(0f, jumpForce));            // ajoute la force du saut
            }
            if (jumpAmount == 1)                                    // double saut
            {
                if (role == Role.BLANC)
                {
                    rb.velocity -= new Vector2(0, rb.velocity.y);
                    rb.AddForce(new Vector2(0f, DoubleJumpForce));
                }
                if (role == Role.NOIR)                              // le player n'a pas accès au double saut, donc l'ajout de force est de 0
                {
                    rb.AddForce(new Vector2(0f, jumpForce * 0f));
                }
            }
            isJuming = false;
        }
    }

    [PunRPC]
    public void FlipToggleSprite(bool flipState)
    {
        GetComponent<SpriteRenderer>().flipX = flipState;
    }
    #endregion

    #region Scream

    /// <summary>
    /// Demande au joueur de débloquer un cri spécifié. Ce dernier est débloqué seulement si autorisé
    /// </summary>
    /// <param name="scream">Le cri à débloquer</param>
    /// <returns>'True' si le cri à été débloqué, 'false' autrement</returns>
    public bool UnlockScream(ScreamType scream)
    {
        //On vérifie dans la structure de donnée si le cri est autorisé pour ce rôle, et qu'il n'est pas débloqué...
        if(screamsDataParsed[scream].unlockableBy.Equals(this.role) && !unlockedScreams.Contains(scream))
        {
            //..si oui, on le débloque.
            unlockedScreams.Add(scream);
            GameEvents.Instance.TriggerNewScreamEvent(this, scream);
            return true;
        }
        //...sinon, on ne fait rien.
        return false;
    }

    /// <summary>
    /// Monte ou descend dans les index des cris appris pour selectionner un cri.
    /// </summary>
    /// <param name="isUp">Si l'index doit monter ou descendre dans la liste des cris appris</param>
    public void ScreamSelection(bool isUp)
    {
        if(unlockedScreams.Count != 0)
        {
            if (isUp)
                screamUnlockedIndex = (screamUnlockedIndex + 1) % unlockedScreams.Count;
            else
                screamUnlockedIndex = ((screamUnlockedIndex - 1) % unlockedScreams.Count + unlockedScreams.Count) % unlockedScreams.Count;

            selectedScream = unlockedScreams[screamUnlockedIndex];
            GameEvents.Instance.TriggerChangeSelectedScreamEvent(this, selectedScream);
        }
    }

    /// <summary>
    /// When the scream key is pressed, call the method associated with the selected scream. Call the RPC version if online.
    /// Need some rework (No need to not call RPC).
    /// </summary>
    public void Screaming()
    {
        if (!isScreaming && photonView.isMine)
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

    /// <summary>
    /// Callbacks called when a fireScreamAbility is triggered. Call the right Callbacks depending on the ScreamType
    /// </summary>
    /// <param name="player">Player emitting the scream</param>
    /// <param name="scream">The scream emitted</param>
    void OnFireScreamAbility(Player player, ScreamType scream)
    {
        switch(scream)
        {
            case ScreamType.Cornered: OnCorneredScream(); break;
            case ScreamType.Pride: OnPrideScream(); break;
        }

    }

    IEnumerator WaitDuringScreaming()
    {
        isScreaming = true;
        yield return new WaitForSeconds(1);
        isScreaming = false;
    }
    #endregion

    #region ScreamsRPC
    [PunRPC]
    public void CompassionScream()
    {
        // endgame
    }

    [PunRPC]
    public void CorneredScream()
    {
        screamsDataParsed[ScreamType.Cornered].sound.Post(gameObject);

        if (this.role == Role.NOIR)
        {
            GameEvents.Instance.TriggerFireScreamAbilityEvent(this, ScreamType.Cornered);
            anim.SetLayerWeight(2, 1f);
            capsuleCollider.size = defaultCapsuleColliderSize * 1.75f;
            groundCheckingCenter.y = defaultGroundCenter.y - 0.25f;
            groundCheckingRadius = defaultGroundRadius * 1.75f;
            StartCoroutine(CorneredWait());
        }        
    }

    private void OnCorneredScream()
    {
        if (this.role == Role.BLANC)
        {
            anim.SetLayerWeight(1, 1f);
            capsuleCollider.size = defaultCapsuleColliderSize * 0.5f;
            groundCheckingCenter.y = defaultGroundCenter.y + 0.15f;
            groundCheckingRadius = defaultGroundRadius * 0.5f;
            StartCoroutine(CorneredWait());
        }
    }

    IEnumerator CorneredWait()
    {
        yield return new WaitForSeconds(5);

        if (this.role == Role.BLANC)
        {
            anim.SetLayerWeight(1, 0f);
            capsuleCollider.size = defaultCapsuleColliderSize;
            groundCheckingCenter.y = defaultGroundCenter.y;
            groundCheckingRadius = defaultGroundRadius;
        }

        if (this.role == Role.NOIR)
        {
            anim.SetLayerWeight(2, 0f);
            capsuleCollider.size = defaultCapsuleColliderSize;
            groundCheckingCenter.y = defaultGroundCenter.y;
            groundCheckingRadius = defaultGroundRadius;
        }
    }

    [PunRPC]
    public void CuriosityScream()
    {
        screamsDataParsed[ScreamType.Curiosity].sound.Post(gameObject);
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
        screamsDataParsed[ScreamType.Joy].sound.Post(gameObject);
        jumpAmount = 2;
    }

    [PunRPC]
    public void PrideScream()
    {
        screamsDataParsed[ScreamType.Pride].sound.Post(gameObject);

        if (this.role == Role.BLANC)
        {
            GameEvents.Instance.TriggerFireScreamAbilityEvent(this, ScreamType.Pride);
            anim.SetLayerWeight(2, 1f);
            capsuleCollider.size = defaultCapsuleColliderSize * 1.75f;
            groundCheckingCenter.y = defaultGroundCenter.y - 0.25f;
            groundCheckingRadius = defaultGroundRadius * 1.75f;
            StartCoroutine(PrideWait());
        }
    }

    IEnumerator PrideWait()
    {
        yield return new WaitForSeconds(5);

        if (this.role == Role.BLANC)
        {
            anim.SetLayerWeight(2, 0f);
            capsuleCollider.size = defaultCapsuleColliderSize;
            groundCheckingCenter.y = defaultGroundCenter.y;
            groundCheckingRadius = defaultGroundRadius;
        }

        if (this.role == Role.NOIR)
        {
            anim.SetLayerWeight(1, 0f);
            capsuleCollider.size = defaultCapsuleColliderSize;
            groundCheckingCenter.y = defaultGroundCenter.y;
            groundCheckingRadius = defaultGroundRadius;
        }
    }

    private void OnPrideScream()
    {
        if (this.role == Role.NOIR)
        {
            anim.SetLayerWeight(1, 1f);
            capsuleCollider.size = defaultCapsuleColliderSize * 0.5f;
            groundCheckingCenter.y = defaultGroundCenter.y + 0.15f;
            groundCheckingRadius = defaultGroundRadius * 0.5f;
            StartCoroutine(PrideWait());
        }
    }

    [PunRPC]
    public void SadnessScream()
    {
        screamsDataParsed[ScreamType.Sadness].sound.Post(gameObject);
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
        screamsDataParsed[ScreamType.Solitude].sound.Post(gameObject);
        foreach (GameObject platform in GameObject.FindGameObjectsWithTag(solitudeScreamReceiversTag))
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
    private void OnEnable()             // permet au Controls de fonctionner
    {
        control.Deplacement.Enable();
        control.Cri.Enable();
    }
    private void OnDisable()
    {
        control.Deplacement.Disable();
        control.Cri.Disable();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion

    #region Animation Events 
    public void PlayFootstep()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + groundCheckingCenter, Vector2.down, groundCheckingRadius, humidityLayer);

        if (hit.collider)
        {
            footStepsWetRTPC.SetGlobalValue(100f);
        }
        else
        {
            footStepsWetRTPC.SetGlobalValue(0f);
        }

        footStepsWetRTPC.Validate();
        playerMoveSound.Post(gameObject);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)groundCheckingCenter, groundCheckingRadius);
        Gizmos.DrawLine(transform.position + (Vector3)sadnessGroundCheckingCenter, transform.position + Vector3.down * sadnessScreamMinGroundDistance);
    }
    #endregion
}