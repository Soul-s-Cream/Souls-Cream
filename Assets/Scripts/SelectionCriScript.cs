using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionCriScript : MonoBehaviour
{
    public List<GameObject> Cris;
    public int criSelected = 0;
    private Vector3 Rot;
    private Vector3 scaleCri;

    #region
    private Controls control;
    private void Awake()
    {
        control = new Controls();
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
    #endregion

    private void Start()
    {
        Rot = transform.eulerAngles;
        scaleCri = Cris[criSelected].transform.localScale;
    }
    private void Update()
    {
        if (control.Cri.CriUp.triggered)
        {
            if (criSelected < 4)
            {
                criSelected += 1;
            }
            else criSelected = 0;

            RotationUpVoid();
            AnimIcon();
        }
        if (control.Cri.CriDown.triggered)
        {
            if (criSelected > 0)
            {
                criSelected -= 1;
            }
            else criSelected = 4;
            RotationDownVoid();
            AnimIcon();
        }
    }

    private void RotationUpVoid()
    {
        Rot.z += 72;
        transform.DORotate(Rot, 1f);
    }
    private void RotationDownVoid()
    {
        Rot.z -= 72;
        transform.DORotate(Rot, 1f);
    }

    public void AnimIcon()
    {
        GameEvents.Instance.SwitchIconeUp(Cris[criSelected]);

        foreach (GameObject icone in Cris)
        {
            if (icone != Cris[criSelected])
            {
                GameEvents.Instance.SwitchIconeDown(icone);
            }
        }


    }
}
