using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualMessageTrigger : MonoBehaviour
{
    [SerializeField]
    private string message = "Default message";

    [SerializeField]
    private float messageDuration = 1f;

    public static event Action<string, float> ContextualMessageTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(ContextualMessageTriggered != null)
            {
                ContextualMessageTriggered.Invoke(message, messageDuration);
            }
        }
    }
}
