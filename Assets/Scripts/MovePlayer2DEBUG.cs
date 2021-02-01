using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer2DEBUG : MonoBehaviour
{
    #region
    private Controls control;
    public List<BoxController> boxes;
    public bool CriN1 = false;
    public float dist;
    public float distMax = 4f;

    private void Awake()
    {
        control = new Controls();
    }
    private void OnEnable()
    {
        control.DEBUG.Enable();
        control.Cri.Enable();
    }
    private void OnDisable()
    {
        control.DEBUG.Disable();
        control.Cri.Disable();
    }
    #endregion  //Controls
    #region
    public int NumSaut = 0;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float velocityYMax = 50f;

    public float moveSpeed; // vitesse de déplacement
    public float jumpForce;
    public float maxSpeed;

    public bool isJuming;
    public bool isGrounded;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement = 0f;
    #endregion  // déplacement
    Animator anim;
    private Vector3 scale;


    public int CriSelected = 1;
    public int CriNumMax = 3;
    private void Start()
    {
        anim = GetComponent<Animator>();
        scale = transform.eulerAngles;
    }
    void FixedUpdate()
    {
        transform.eulerAngles = scale;
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);
        horizontalMovement = control.DEBUG.DeplacementJ2.ReadValue<float>() * moveSpeed;
        PlayerMove(horizontalMovement);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));



        if (control.DEBUG.DeplacementJ2.ReadValue<float>() == 1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (isGrounded)
            {
                AkSoundEngine.PostEvent("OzFootsteps", gameObject);
            }
        }
        if (control.DEBUG.DeplacementJ2.ReadValue<float>() == -1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (isGrounded)
            {
                AkSoundEngine.PostEvent("OzFootsteps", gameObject);
            }
        }
        if(isGrounded && control.DEBUG.DeplacementJ2.ReadValue<float>()== 0)
        {
            AkSoundEngine.PostEvent("OzLanding", gameObject);
        }


    }
    private void Update()
    {

        if (velocity.y > velocityYMax)
        {
            velocity.y = velocityYMax;
        }
        if (control.DEBUG.JumpJ2.triggered && NumSaut < 1)
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
        else if (rb.velocity.y > 0 && !control.DEBUG.JumpJ2.triggered)
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
                AkSoundEngine.PostEvent("OzJump", gameObject);

                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (NumSaut == 1)
            {
                AkSoundEngine.PostEvent("OzDoubleJump", gameObject);
                rb.AddForce(new Vector2(0f, jumpForce / 2));
            }
            isJuming = false;
        }

    }

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

        if (control.Cri.CriOzWhite.triggered)
        {
            anim.SetInteger("Cri", CriSelected);
            if (CriSelected == 1)               //Acculé
            {
                CriN1 = true;

                /*foreach (BoxController box in boxes)
                {
                    dist = Vector2.Distance(box.transform.position, transform.position);
                    if (dist <= distMax)
                    {
                        GameEvents.Instance.SwitchBoxOn(box);
                        Debug.Log("la boite peux bouger");
                    }
                    else Debug.Log("Je suis trop loin");
                }*/
                AkSoundEngine.PostEvent("Compassion", gameObject);
            }
            if (CriSelected == 2)
            {
                AkSoundEngine.PostEvent("Curiosite", gameObject);
            }
            if (CriSelected == 3)
            {
                AkSoundEngine.PostEvent("Fierte", gameObject);
            }
            if (CriSelected == 4)
            {
                AkSoundEngine.PostEvent("Joie", gameObject);
            }


        }
        else
        {
            anim.SetInteger("Cri", 0);
            CriN1 = false;
        }

    }
}
