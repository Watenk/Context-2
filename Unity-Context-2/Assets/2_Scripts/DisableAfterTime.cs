using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float timeToWait;
    private void OnEnable()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
