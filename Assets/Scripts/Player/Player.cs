using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Public Field
    public int jumpAmount = 0;

    public float moveSpeed;
    public float moveSmooth = .05f;
    public float jumpForce;
    public float maxSpeed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float velocityYMax = 50f;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;

    [Header("Transform where the screams will be instantiate")]
    public Transform screamCenter;
    [Header("List of scream (must be located in the Resources folder)")]
    public List<Scream> screams;

    private int selectedScream = 0;
    public Role role;
    #endregion  //Controls

    #region Private Fields
    private bool isJuming;
    private bool isGrounded;
    private Animator anim;
    private Rigidbody2D rb;
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
        rb = GetComponent<Rigidbody2D>();
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

    #region Scream
    public void ScreamSelection()
    {
        if (control.Cri.CriUp.triggered)
        {
            selectedScream = (selectedScream + 1) % screams.Count;
        }
        if (control.Cri.CriDown.triggered)
        {
            selectedScream = (selectedScream - 1) % screams.Count;
        }
    }
    public void Screaming()
    {
        if (control.Cri.Cri.triggered)
        {
            anim.SetInteger("Scream", selectedScream);
            PhotonNetwork.Instantiate(screams[selectedScream].name, screamCenter.position, screamCenter.rotation, 0);
        }
        else
        {
            anim.SetInteger("Scream", 0);
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