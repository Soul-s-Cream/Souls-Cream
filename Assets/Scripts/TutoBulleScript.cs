using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutoBulleScript : MonoBehaviour        // scipte à attacher quelque par dans le tuto
{
    private Controls control;

    public Image JumpSprite_01;                    // différentes incones des touches du jeu, se sont des images dans un canevas
    public Image DeplacementLeftSprite_02;
    public Image DeplacementRightSprite_03;
    public Image CriSprite_04;
    public Image ChangeCriUpSprite_05;
    public Image ChangeCriDownSprite_06;

    public bool jumpControlIsPress_01 = false;          // permet de géréer la progression du tuto
    public bool moveLeftIsPress_02 = false;
    public bool moveRightIsPress_03 = false;
    public bool criControlIsPress_04 = false;
    public bool switchCriUpIsPress_05 = false;
    public bool switchCriDownIsPress_06 = false;

    public bool actif = false;
    private float waitTime = 1.5f;


    public Image EcranOpaque;
    public float ecranAlpha = .3f;
    private bool lalaJattend;
    private Color ecranTutoAlphaColor;
    private bool ecranTutoActif = true;

    private void Awake()
    {
        control = new Controls();
        JumpSprite_01.GetComponent<Image>().DOColor(Color.white, 1f);
        ecranTutoAlphaColor = new Color(1, 1, ecranAlpha);
    }
    private void Update()               // lorsqu'on actionne les différent contrôles, l'image du contrôle s'efface et celle du controle suivant s'affiche
    {
        if (!actif)
        {

            if (control.Deplacement.Jump.triggered && !jumpControlIsPress_01)
            {
                TutoJump();
                StartCoroutine(lalalala());
            }

            if (jumpControlIsPress_01 && !moveLeftIsPress_02 && control.Deplacement.Deplacement.ReadValue<float>() < 0f)
            {
                TutoMoveLeft();
                StartCoroutine(lalalala());
            }
            if (moveLeftIsPress_02 && !moveRightIsPress_03 && control.Deplacement.Deplacement.ReadValue<float>() > 0f)
            {
                TutoMoveRight();
                StartCoroutine(lalalala());

            }
            if (moveRightIsPress_03 && !criControlIsPress_04 && control.Cri.Cri.triggered)
            {
                TutoCri();
                StartCoroutine(lalalala());

            }
            if (criControlIsPress_04 && !switchCriUpIsPress_05 && control.Cri.CriUp.triggered)
            {
                TutoSwitchCriUp();
                StartCoroutine(lalalala());
            }
            if (switchCriUpIsPress_05 && !switchCriDownIsPress_06 && control.Cri.CriDown.triggered)
            {
                TutoSwitchCriDown();
                StartCoroutine(lalalala());
            }

            if (control.Cri.Cri.triggered)
            {
                EcranTutoVoid();
            }
        }

        /*
        if (jumpControlIsPress_01 && moveLeftIsPress_02 && JumpSprite_01.GetComponent<Image>().color != Color.clear)
        {
            JumpSprite_01.GetComponent<Image>().color = Color.black;
        }
        if (moveLeftIsPress_02 && moveRightIsPress_03 && DeplacementLeftSprite_02.GetComponent<Image>().color != Color.clear)
        {
            DeplacementLeftSprite_02.GetComponent<Image>().color = Color.black;
        }
        if (moveRightIsPress_03 && CriControlIsPress_04 && DeplacementRightSprite_03.GetComponent<Image>().color != Color.clear)
        {
            DeplacementRightSprite_03.GetComponent<Image>().color = Color.black;
        }
        if (CriControlIsPress_04 && SwitchCriUpIsPress_05 && CriSprite_04.GetComponent<Image>().color != Color.clear)
        {
            CriSprite_04.GetComponent<Image>().color = Color.black;
        }
        if (SwitchCriUpIsPress_05 && SwitchCriDownIsPress_06 && ChangeCriUpSprite_05.GetComponent<Image>().color != Color.clear)
        {
            ChangeCriUpSprite_05.GetComponent<Image>().color = Color.black;
        }*/
        
    }
    private void TutoJump()
    {
        JumpSprite_01.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        DeplacementLeftSprite_02.GetComponent<Image>().DOColor(Color.white, waitTime);
        jumpControlIsPress_01 = true;
    }
    private void TutoMoveLeft()
    {
        DeplacementLeftSprite_02.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        DeplacementRightSprite_03.GetComponent<Image>().DOColor(Color.white, waitTime);

        //JumpSprite.GetComponent<Image>().color = Color.clear;
        moveLeftIsPress_02 = true;
    }
    private void TutoMoveRight()
    {
        DeplacementRightSprite_03.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        CriSprite_04.GetComponent<Image>().DOColor(Color.white, waitTime);

        //DeplacementLeftSprite.GetComponent<Image>().color = Color.clear;
        moveRightIsPress_03 = true;
    }
    private void TutoCri()
    {
        CriSprite_04.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        ChangeCriUpSprite_05.GetComponent<Image>().DOColor(Color.white, waitTime);

        //DeplacementRightSprite.GetComponent<Image>().color = Color.clear;
        criControlIsPress_04 = true;
    }
    private void TutoSwitchCriUp()
    {
        ChangeCriUpSprite_05.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        ChangeCriDownSprite_06.GetComponent<Image>().DOColor(Color.white, waitTime);

        //CriSprite.GetComponent<Image>().color = Color.clear;
        switchCriUpIsPress_05 = true;
    }
    private void TutoSwitchCriDown()
    {


        ChangeCriDownSprite_06.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        
        //ChangeCriRightSprite.GetComponent<Image>().color = Color.clear;
        switchCriDownIsPress_06 = true;
    }


    IEnumerator lalalala()
    {
        actif = true;
        yield return new WaitForSeconds(waitTime);
        actif = false; 
    }


    private void EcranTutoVoid()
    {

        if (!lalaJattend && ecranTutoActif)
        {
            EcranOpaque.DOColor(ecranTutoAlphaColor, .9f);
            StartCoroutine(WaitTime());
        }
        if (1 == 1 && ecranTutoActif)//si les deux personnages crient en même temps l'écran s'efface
        {
            ecranTutoActif = false;
            EcranOpaque.DOColor(Color.clear, 3f);
            lalaJattend = true;
        }
    }
    IEnumerator WaitTime()
    {
        lalaJattend = true;
        yield return new WaitForSeconds(1f);
        lalaJattend = false;
        EcranOpaque.DOColor(Color.white, 1f);
    }


    #region Unity Call Back
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
