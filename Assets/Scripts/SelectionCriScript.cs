using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionCriScript : MonoBehaviour
{
    public List<GameObject> Cris;
    public int criSelected = 0;
    public bool roueActive = false;

    private Coroutine timeToDie;
    private Vector3 Rot;

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
        StartCoroutine(TimeToDie());
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
            AnimIcon();
            ResetTimeToDie();
            RotationVoid(72);
            roueActive = true;
        }
        if (control.Cri.CriDown.triggered)
        {
            if (criSelected > 0)
            {
                criSelected -= 1;
            }
            else criSelected = 4;
            RotationVoid(-72);
            AnimIcon();
            ResetTimeToDie();
            roueActive = true;
        }
    }

    private void ResetTimeToDie()
    {
        if (timeToDie != null)
        {
            StopCoroutine(timeToDie);
        }
        timeToDie = StartCoroutine(TimeToDie());
    }

    private void RotationVoid(int rotat)
    {

        if (Rot.z + rotat >= 360)
        {
            Rot.z += rotat - 360;
        }
        else if (Rot.z + rotat <= 0)
        {
            Rot.z += rotat + 360;
        } else
            Rot.z += rotat;

        transform.DORotate(Rot, 1f);
    }

    public void AnimIcon()
    {
        GameEvents.Instance.TriggerSwitchIconeUp(Cris[criSelected]);
        foreach (GameObject icone in Cris)
        {
            if (icone != Cris[criSelected])
            {
                GameEvents.Instance.TriggerSwitchIconeDown(icone);
            }
        }
    }
    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(2f);
        roueActive = false;
        GameEvents.Instance.TriggerSwitchIconeDown(Cris[criSelected]);
    }
}
