using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : MonoBehaviour
{/*
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
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


    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = control.Deplacement.Deplacement.ReadValue<float>();

        if (control.Deplacement.Jump.triggered && grounded) // c'est ici qu'on pourra faire le double saut
        {
            velocity.y = jumpTakeOffSpeed; 
        }



        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y = velocity.y * .5f; 
        }
        targetVelocity = move * maxSpeed;
    }*/
}
