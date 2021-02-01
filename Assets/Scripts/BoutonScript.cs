using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonScript : MonoBehaviour
{
    public List<GameObject> triggers;
    public GameObject clef;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == clef.name)
        {
            GameEvents.Instance.SwitchTriggerOn(triggers);
        }
    }
}
