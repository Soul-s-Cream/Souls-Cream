using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer2DEBUG : MonoBehaviour
{
    #region


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
    #region variables


    public List<BoxController> boxes;
    public bool criN1Tristesse = false;
    public bool criN2Accule = false;
    public bool criN3Solitude = false;
    public bool criN4Envie = false;


    public float dist;
    public float distMax = 4f;


    public int NumSaut = 0;
    public int CriSelected = 1;
    public int CriNumMax = 4;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float velocityYMax = 50f;

    public float moveSpeed;
    public float jumpForce;
    public float maxSpeed;

    public bool isJuming;
    public bool isGrounded;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;


    private Controls control;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement = 0f;
    #endregion




    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);
        horizontalMovement = control.DEBUG.DeplacementJ2.ReadValue<float>() * moveSpeed;
        PlayerMove(horizontalMovement);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (control.DEBUG.DeplacementJ2.ReadValue<float>() == 1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (control.DEBUG.DeplacementJ2.ReadValue<float>() == -1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (NumSaut == 1)
            {
                rb.AddForce(new Vector2(0f, jumpForce / 2));
            }
            isJuming = false;
        }

    }

    public void PlayerCriSelect()
    {


        if (control.DEBUG.CriUp.triggered)
        {
            if (CriSelected < CriNumMax)
            { CriSelected += 1; }
            else CriSelected = 1;
        }
        if (control.DEBUG.CriDown.triggered)
        {
            if (CriSelected > 1)
            { CriSelected -= 1; }
            else CriSelected = CriNumMax;
        }
    }
    public void PlayerCri()
    {

        if (control.DEBUG.Cri.triggered)
        {
            anim.SetInteger("Cri", CriSelected);
            if (CriSelected == 1)
            {
                criN1Tristesse = true;
            }
        }
        else
        {
            anim.SetInteger("Cri", 0);
            criN1Tristesse = false;
            criN2Accule = false;
            criN3Solitude = false;
            criN4Envie = false;

        }

    }
}
