using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    #region Public Field
    public List<BoxController> boxes;
    public bool CriN1 = false;
    public float dist;
    public float distMax = 4f;

    public int NumSaut = 0;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float velocityYMax = 50f;

    // vitesse de d�placement
    public float moveSpeed;
    public float jumpForce;
    public float maxSpeed;

    public bool isJuming;
    public bool isGrounded;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    public Rigidbody2D rb;

    public int CriSelected = 1;
    public int CriNumMax = 3;
    public Role role;
    #endregion  //Controls

    #region Private Fields
    Animator anim;
    private Vector3 scale;

    private Vector3 velocity = Vector3.zero;
    // d�placement
    private float horizontalMovement = 0f;

    private Controls control;
    #endregion

    private void Awake()
    {
        control = new Controls();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        scale = transform.eulerAngles;
//        AkSoundEngine.PostEvent("MozTuto", gameObject);
    }

    void FixedUpdate()
    {
        transform.eulerAngles = scale;
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);
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
    private void Update()
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
        if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }
        else anim.SetBool("Jump", true);



        PlayerCriSelect();
        PlayerCri();
    }

    void PlayerMove(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if (isJuming == true)
        {
            if (NumSaut == 0)
            {
                AkSoundEngine.PostEvent("MozJump", gameObject);
                //AkSoundEngine.SetState("OzMozThemeAmbience", "MozBig");
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (NumSaut == 1)
            {

                rb.AddForce(new Vector2(0f, jumpForce * 0f));
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
    }
    public void PlayerCri()
    {

        if (control.Cri.Cri.triggered)
        {
            anim.SetInteger("Cri", CriSelected);
            if (CriSelected == 1)
            {
                CriN1 = true;

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
            anim.SetInteger("Cri", 0);
            CriN1 = false;
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
}