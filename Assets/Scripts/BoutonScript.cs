using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonScript : MonoBehaviour
{
    public List<GameObject> triggers;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameEvents.Instance.SwitchTriggerOn(triggers);
    }
}
