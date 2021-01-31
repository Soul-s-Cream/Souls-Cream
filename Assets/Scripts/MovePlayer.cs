
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    #region
    private Controls control;

    private void Awake()
    {
        control = new Controls();
    }
    private void OnEnable()
    {
        control.Deplacement.Enable();
    }
    private void OnDisable()
    {
        control.Deplacement.Disable();
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

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckLeft.position);

            horizontalMovement = control.Deplacement.Deplacement.ReadValue<float>() * moveSpeed;

       /*if (control.Deplacement.Jump.triggered && isGrounded)
        {
            isJuming = true;
        }*/
        PlayerMove(horizontalMovement);
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

        
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !control.Deplacement.Jump.triggered)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
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
                rb.AddForce(new Vector2(0f, jumpForce/2));
            }
            isJuming = false;
        }

    }
}
