using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiSelectionScript : MonoBehaviour
{
    public GameObject CriSelection;
    SelectionCriScript roue;
    // Start is called before the first frame update
    void Start()
    {
        roue = CriSelection.GetComponent<SelectionCriScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!roue.roueActive)
        {
            GetComponent<SpriteRenderer>().DOColor(Color.clear, 1f);
        }
        else GetComponent<SpriteRenderer>().DOColor(Color.white, 1f);
    }
}
