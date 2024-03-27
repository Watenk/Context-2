using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    SceneSwitcher sceneSwitcher;

    private void Start()
    {
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }


    private void Update()
    {
        if (Input.anyKeyDown)
        {
            sceneSwitcher.SwitchScene(1);
        }
    }


}
