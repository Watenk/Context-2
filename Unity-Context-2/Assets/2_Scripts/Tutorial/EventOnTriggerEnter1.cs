using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter1 : MonoBehaviour
{
    private bool triggerd;
    [SerializeField]
    private float delay;
    [SerializeField]
    private UnityEvent triggerEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (triggerd) { return; }
        triggerd = true;
        StartCoroutine(triggerEventCoroutine());
    }

    private IEnumerator triggerEventCoroutine()
    {
        yield return new WaitForSeconds(delay);
        triggerEvent?.Invoke();
    }
}
