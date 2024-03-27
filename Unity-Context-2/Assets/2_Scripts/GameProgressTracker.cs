using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressTracker : MonoBehaviour
{
    private int progress;



    public void UpdateProgress()
    {
        progress++;
        if(progress >= 9)
        {
            StartCoroutine(DelayedEnding());
        }
    }

    private void StartOutro()
    {
        FindObjectOfType<SceneSwitcher>().SwitchScene(3);
    }

    private IEnumerator DelayedEnding()
    {
        yield return new WaitForSeconds(4);
        StartOutro();
    }
}
