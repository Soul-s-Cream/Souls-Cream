using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class PlayerTestNet : Photon.PunBehaviour
{
    Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.NetTest.Jump.performed += ctx => Jump();
        controls.NetTest.ChangeScene.performed += ctx => ChangeScene();
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
        if(photonView.isMine)
            transform.DOJump(this.transform.position, 1f, 1, 1f);
    }

    private void ChangeScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
            PhotonNetwork.LoadLevel(1);
        else
            PhotonNetwork.LoadLevel(0);
    }
}
