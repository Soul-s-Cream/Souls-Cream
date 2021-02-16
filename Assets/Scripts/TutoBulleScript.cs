using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutoBulleScript : MonoBehaviour
{
    private Controls control;

    public Image JumpSprite;
    public Image DeplacementRightSprite;
    public Image DeplacementLeftSprite;
    public Image CriSprite;
    public Image ChangeCriRightSprite;
    public Image ChangeCriLeftSprite;

    public bool spaceIsPress = false;
    private bool qIsPress = false;
    private bool dIsPress = false;
    private bool rIsPress = false;
    private bool eIsPress = false;
    private bool aIsPress = false;

    private void Awake()
    {
        control = new Controls();
        JumpSprite.GetComponent<Image>().DOColor(Color.white, 1f);
    }
    private void Update()
    {

        if (control.Deplacement.Jump.triggered && !spaceIsPress)
        {

                JumpSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
                DeplacementLeftSprite.GetComponent<Image>().DOColor(Color.white, 2f);
                spaceIsPress = true;
        }

        if (spaceIsPress && !qIsPress && control.Deplacement.Deplacement.ReadValue<float>() == 1f)
        {
            DeplacementLeftSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
            DeplacementRightSprite.GetComponent<Image>().DOColor(Color.white, 2f);
            qIsPress = true;

        }
        if (qIsPress && !dIsPress && control.Deplacement.Deplacement.ReadValue<float>() == -1f)
        {
            DeplacementRightSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
            CriSprite.GetComponent<Image>().DOColor(Color.white, 2f);
            dIsPress = true;
        }
        if (dIsPress && !rIsPress && control.Cri.Cri.triggered)
        {
            CriSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
            ChangeCriRightSprite.GetComponent<Image>().DOColor(Color.white, 2f);
            rIsPress = true;
        }
        if (rIsPress && !eIsPress && control.Cri.CriUp.triggered)
        {
            ChangeCriRightSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
            ChangeCriLeftSprite.GetComponent<Image>().DOColor(Color.white, 2f);
            eIsPress = true;
        }
        if (eIsPress && !aIsPress && control.Cri.CriDown.triggered)
        {
            ChangeCriLeftSprite.GetComponent<Image>().DOColor(Color.clear, 0.5f);
            aIsPress = true;
        }

    }
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
}
