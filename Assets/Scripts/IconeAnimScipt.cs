using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IconeAnimScipt : MonoBehaviour
{
    private Vector3 scaleCri;

    // Start is called before the first frame update
    void Start()
    {    
        GameEvents.Instance.iconAnimSelected += IconeActive;
        GameEvents.Instance.iconAnimUnselected += IconeDesactive;

        scaleCri = transform.localScale;
    }

    void IconeActive(GameObject thisIcone)
    {
        if (thisIcone == this.gameObject)
        {
            scaleCri.x = 0.75f; scaleCri.y = 0.75f;
            transform.DOScale(scaleCri, 1f);
            GetComponent<SpriteRenderer>().DOColor(Color.white, 1f);

        }
    }
    void IconeDesactive(GameObject thisIcone)
    {
        if (thisIcone == this.gameObject)
        {
            scaleCri.x = 0.50f; scaleCri.y = 0.50f;
            transform.DOScale(scaleCri, 1f);
            GetComponent<SpriteRenderer>().DOColor(Color.clear, 1f);
        }
    }
}
