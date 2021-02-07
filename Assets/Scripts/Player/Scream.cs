using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Scream : MonoBehaviour
{
    [Header("The radius of the scream when it hit something")]
    public float radius;
    [Header("The layer used to hit this scream with scream receiver(s) (dont forget to change the layer of receiver(s)")]
    public LayerMask hitLayer;
    [Header("If true, the gameobject will be destroyed after gameplay logics computed")]
    public bool destroyAfterEmit;
    [Header("Optionnal event invoke after gameplay logics computed")]
    public UnityEvent onEmit;
    [Header("Optionnal AkSoundEngine event invoke after gameplay logics computed")]
    public string akEventName;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere((Vector2)transform.position, radius, hitLayer);

        foreach (Collider collider in colliders)
        {
            ScreamReceiver screamReceiver = collider.GetComponent<ScreamReceiver>();

            if (screamReceiver)
            {
                GameEvents.Instance.receiveScream(screamReceiver);
            }
        }

        onEmit.Invoke();

        if (akEventName != "")
        {
            AkSoundEngine.PostEvent(akEventName, gameObject);
        }

        if (destroyAfterEmit)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
