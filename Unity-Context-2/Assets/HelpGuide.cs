using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpGuide : MonoBehaviour
{
    public GameObject guideGameObject;

    void Start(){
        EventManager.AddListener(Events.OnMap, OnMapInput);
    }

    private void OnMapInput(){
        if (guideGameObject.activeSelf){
            guideGameObject.SetActive(false);
        }
        else{
            guideGameObject.SetActive(true);
        }
    }
}
