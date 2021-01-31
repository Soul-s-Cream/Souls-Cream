
using UnityEngine;

public class MovePlayer2DEBUG : MonoBehaviour
{
    #region
    private Controls control;

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
            scale.y = 180;
        }
        if (control.DEBUG.DeplacementJ2.ReadValue<float>() == -1)
        {
            scale.y = 0;
        }
    }
    private void Update()
    {
        Debug.Log(control.Cri.CriDown.triggered);
        Debug.Log(control.Cri.CriUp.triggered);
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

    public void PlayerCri()
    {
        Debug.Log(control.Cri.CriDown.triggered);
        Debug.Log(control.Cri.CriUp.triggered);

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

}
