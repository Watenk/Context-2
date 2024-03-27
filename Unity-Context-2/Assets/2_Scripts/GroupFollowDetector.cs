using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroupFollowDetector : MonoBehaviour
{
    [SerializeField]
    private UnityEvent triggerEvent;
    private bool isTriggered;

    void Start(){
        GameManager.GetService<CommunityManager>().OnFollow += OnFollow;
    }

    private void OnFollow(CommunityTypes community){
        Debug.Log("OnFollow");
        if (isTriggered) return;
        triggerEvent?.Invoke();
        isTriggered = true;
    }
}
