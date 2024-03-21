using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter : MonoBehaviour
{
    private bool triggerd;
    [SerializeField]
    private float delay;
    [SerializeField]
    private UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerd)
        {
            triggerd = true;
            StartCoroutine(triggerEventCoroutine());
        }
    }

    private IEnumerator triggerEventCoroutine()
    {
        yield return new WaitForSeconds(delay);
        triggerEvent?.Invoke();
    }
}
