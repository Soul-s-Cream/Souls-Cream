using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerTestNet : MonoBehaviour
{
    Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.NetTest.Jump.performed += ctx => Jump();
    }

    private void OnEnable()
    {
        controls.NetTest.Enable();
    }

    private void OnDisable()
    {
        controls.NetTest.Disable();
    }

    void Jump()
    {
            transform.DOJump(this.transform.position, 1f, 1, 1f);
    }
}
