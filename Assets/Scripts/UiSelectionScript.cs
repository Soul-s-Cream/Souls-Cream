using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiSelectionScript : MonoBehaviour
{
    public SelectionCriScript selectionCriScript;

    // Update is called once per frame
    void Update()
    {
        if (!selectionCriScript.roueActive)
        {
            GetComponent<SpriteRenderer>().DOColor(Color.clear, 1f);
        }

        else GetComponent<SpriteRenderer>().DOColor(Color.white, 1f);
    }
}
