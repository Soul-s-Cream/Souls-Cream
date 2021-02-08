using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerNet : Photon.PunBehaviour
{
    #region Public Field
    public List<MoveableBox> boxes;
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
    }

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            return;
        }

        transform.eulerAngles = scale;
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);
        horizontalMovement = control.Deplacement.Deplacement.ReadValue<float>() * moveSpeed;
        PlayerMove(horizontalMovement);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (control.Deplacement.Deplacement.ReadValue<float>() == 1)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, true);
            
        }
        if (control.Deplacement.Deplacement.ReadValue<float>() == -1)
        {
            photonView.RPC("FlipToggleSprite", PhotonTargets.All, false);
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
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            if (NumSaut == 1)
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
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), distMax);

                foreach (Collider2D collider in colliders)
                {
                    if(collider.gameObject.CompareTag("Crate"))
                        GameEvents.Instance.TriggerSwitchBoxOn(collider.GetComponent<MoveableBox>());
                }
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