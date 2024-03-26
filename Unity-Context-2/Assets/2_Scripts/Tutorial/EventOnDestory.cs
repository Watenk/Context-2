using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDestory : MonoBehaviour
{
    [SerializeField]
    private UnityEvent triggerEvent;

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) 
        {
            triggerEvent?.Invoke();
        }
    }
}
