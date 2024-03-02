using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    public Sound GenerateSound(AK.Wwise.Event startEvent, AK.Wwise.Event stopEvent, Vector3 pos){
        GameObject soundInstance = GameObject.Instantiate(new GameObject(), pos, Quaternion.identity);
        soundInstance.transform.SetParent(GameManager.Instance.transform);
        startEvent.Post(soundInstance);
        return new Sound(soundInstance, stopEvent);
    }
}
