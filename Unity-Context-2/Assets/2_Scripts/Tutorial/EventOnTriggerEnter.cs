using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter : MonoBehaviour
{
    public ChimeSequences chimeSequence;

    private bool triggerd;
    [SerializeField]
    private float delay;
    [SerializeField]
    private UnityEvent triggerEvent;


    IEnumerator Start(){
        yield return null;
        GameManager.GetService<MuralManager>().GetMural(chimeSequence).OnSequenceDone += OnChimeSequence;
    }

    private void OnChimeSequence(){
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
