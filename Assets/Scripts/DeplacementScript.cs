using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementScript : MonoBehaviour
{    [Range(1, 10)]
    public float jumpVelocity;

    private Controls control;
    private Vector3 position;



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

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position.x += control.Deplacement.Deplacement.ReadValue<float>();
        //transform.position.x = position.x;
        if (control.Deplacement.Jump.triggered)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
        if (control.Deplacement.Deplacement.ReadValue< float>() == 1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * jumpVelocity;
        }
        if (control.Deplacement.Deplacement.ReadValue<float>() == -1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left * jumpVelocity;
        }
    }
}
