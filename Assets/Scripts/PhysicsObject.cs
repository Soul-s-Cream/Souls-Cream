using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;
    public float minGroundNormalY = .65f;

    protected Vector2 targetVelocity; 
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;                                         //Vitesse actuelle de l'objet
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];          // list des objet au alentour
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;                     // distance minimum entre l'objet et celui qui entre en collision
    protected const float shellRadius = 0.01f;

    public Vector3 Position;
    public float Vitesse;


    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }


    void Start()
    {
        contactFilter.useTriggers = false;
        //Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }
        protected virtual void ComputeVelocity()
    {

    }
    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x; 
        grounded = false;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = moveAlongGround * deltaPosition.x;
        Mouvement(move, false);


        move = Vector2.up * deltaPosition.y;

        Mouvement(move, true);
    }
    void Mouvement(Vector2 move, bool yMouvement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
           int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for (int i = 0; i <hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMouvement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projetion = Vector2.Dot(velocity, currentNormal);
                if (projetion < 0)
                {
                    velocity = velocity - projetion * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
