using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreamReceiver : MonoBehaviour
{
    [Header("Event invoked when this object is hit by a scream (hit layer is specify in the scream script)")]
    public UnityEvent onReceived;

    public void Receive()
    {
        onReceived.Invoke();
    }
}
