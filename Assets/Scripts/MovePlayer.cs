using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour             //Script attache au joueur noir
{
    #region Public Field
    public List<BoxController> boxes;

    public bool criN1Joie = false;
    public bool criN2Fierte = false;
    public bool criN3Curiosite = false;
    public bool criN4Compassion = false;
    public bool isJuming;
    public bool isGrounded;
    public bool chuteLibre = false;
    public bool freezeMove = false;

    public float distMax = 4f;
    public float velocityYMax = 50f;
    public float moveSpeed;
    public float jumpForce;
    public float maxSpeed;



    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    public Rigidbody2D rb;

    public int CriSelected = 1;
    public int CriNumMax = 3;
    public Role role;
    #endregion  //Controls

    #region Private Fields

    private Animator anim;
    private Controls control;
    private Coroutine timeToDie;

    private Vector3 velocity = Vector3.zero;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private float dist;
    private float horizontalMovement = 0f;

    private bool dernierCriActive = false;

    private int NumSaut = 0;

    #endregion

    private void Awake()
    {
        control = new Controls();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        //        AkSoundEngine.PostEvent("MozTuto", gameObject);
    }

    void FixedUpdate()
    {
        if (!freezeMove)
        {
            isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);                 // le personnage est considere au sol
            horizontalMovement = control.Deplacement.Deplacement.ReadValue<float>() * moveSpeed;
            PlayerMove(horizontalMovement);

            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            if (control.Deplacement.Deplacement.ReadValue<float>() == 1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (isGrounded)
                {
                    AkSoundEngine.PostEvent("MozFootsteps", gameObject);
                }
            }
            if (control.Deplacement.Deplacement.ReadValue<float>() == -1)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (isGrounded)
                {
                    AkSoundEngine.PostEvent("MozFootsteps", gameObject);
                }
            }
            if (isGrounded && control.Deplacement.Deplacement.ReadValue<float>() == 0)
            {
                //AkSoundEngine.PostEvent("MozLanding", gameObject);
            }
        }
    }
    private void Update()                                   // a mettre dans la classe
    {
        if (!freezeMove)
        {
            if (velocity.y > velocityYMax)
            {
                velocity.y = velocityYMax;
            }
            if (control.Deplacement.Jump.triggered && NumSaut < 1)
            {
                isJuming = true;
                NumSaut += 1;
            }
            if (isGrounded)
            {
                NumSaut = 0;
            }
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !control.Deplacement.Jump.triggered)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            if (isGrounded && !chuteLibre)
            {
                anim.SetBool("Jump", false);
            }
            else anim.SetBool("Jump", true);



            PlayerCriSelect();
            PlayerCri();
        }
    }

    void PlayerMove(float _horizontalMovement)                                  // a mettre dans la classe Player
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if (isJuming == true && !chuteLibre)
        {
            if (NumSaut == 0)
            {
                AkSoundEngine.PostEvent("MozJump", gameObject);
                //AkSoundEngine.SetState("OzMozThemeAmbience", "MozBig");
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (NumSaut == 1)
            {

                rb.AddForce(new Vector2(0f, jumpForce * 0f));                   // !!! valeur de 0 pour le joueur noir !!!
            }
            isJuming = false;
        }

    }

    #region Cri
    public void PlayerCriSelect()
    {

        if (control.Cri.CriUp.triggered)
        {
            if (CriSelected < CriNumMax)
            { CriSelected += 1; }
            else CriSelected = 1;
        }
        if (control.Cri.CriDown.triggered)
        {
            if (CriSelected > 1)
            { CriSelected -= 1; }
            else CriSelected = CriNumMax;
        }
    }                                                           // polish : essayer de changer cette valeur en fonction du script SelectionCriScript /////// a mettre dans la classe player
    public void PlayerCri()
    {

        if (control.Cri.Cri.triggered)
        {
            anim.SetInteger("Cri", CriSelected);                                                // utilise l'animation du cri du perso
            ResetTimeToDie();                                                                   // Coroutine de déactivation du cri



            if (CriSelected == 1)                                                               // Cri de déplacement de bloc
            {
                criN1Joie = true;
                foreach (BoxController box in boxes)
                {
                    dist = Vector2.Distance(box.transform.position, transform.position);
                    if (dist <= distMax)
                    {
                        GameEvents.Instance.SwitchBoxOn(box);
                    }
                }
                AkSoundEngine.PostEvent("Acculation", gameObject);
            }
            if (CriSelected == 2)
            {
                AkSoundEngine.PostEvent("Envie", gameObject);
            }
            if (CriSelected == 3)
            {
                AkSoundEngine.PostEvent("Solitude", gameObject);
            }
            if (CriSelected == 4)
            {
                AkSoundEngine.PostEvent("Tristesse", gameObject);
            }


        }
        else
        {
            anim.SetInteger("Cri", 0);                                      // personnage en position de IDLE
        }

    }

    public void DernierCri()                        // s'active au cour de la dernière cinematique
    {
        dernierCriActive = true;
    }


    private IEnumerator TimeToDie()                 //Permet de déactive le pouvoir du cri
    {
        yield return new WaitForSeconds(2f);
        criN1Joie = false;
        criN2Fierte = false;
        criN3Curiosite = false;
        criN4Compassion = false;
    }
    private void ResetTimeToDie()
    {
        if (timeToDie != null)
        {
            StopCoroutine(timeToDie);
        }
        timeToDie = StartCoroutine(TimeToDie());
    }

    public void ChuteLibre()                    // a ajouter dans la classe Player
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
}