using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage_Script : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public float gravityModifier = 1f;

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
        //transform.position = Position;

        /*if (Input.GetKey(KeyCode.Z))
        {
            Position.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Position.y -= 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Position.x -= Vitesse;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Position.x += Vitesse;
        }*/
        
    }
    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = Vector2.up * deltaPosition.y;
        Mouvement(move);
    }
    void Mouvement(Vector2 move)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            rb2d.Cast(move, contactFilter, hitBuffer, distance +)
        }
        rb2d.position = rb2d.position + move;
    }
}
