using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : IUpdateable
{
    private GameObject soundObject;
    private List<GameObject> soundObjects = new List<GameObject>();

    // References
    private InputManager inputManager;

    //-----------------------------------------

    public AudioManager(){
        inputManager = GameManager.Instance.GetService<InputManager>();
    }

    public void OnUpdate(){

    }

    public void CallSoundEvent(AK.Wwise.Event wwiseEvent, Vector3 pos){
        GameObject soundInstance = InstanceSound(pos);
        soundInstance.transform.SetParent(GameManager.Instance.transform);
        soundObjects.Add(soundInstance);
        wwiseEvent.Post(soundInstance);
    }

    //-----------------------------------

    private GameObject InstanceSound(Vector3 pos){
        return GameObject.Instantiate(soundObject, pos, Quaternion.identity);
    }
}
