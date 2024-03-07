using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IFixedUpdateable
{
    private List<LoopingSound> loopingSounds = new List<LoopingSound>();
    private List<Sound> sounds = new List<Sound>();
    private GameObject soundPrefab;

    //-------------------------------------------------------

    public SoundManager(){
        soundPrefab = AudioSettings.Instance.SoundPrefab;

        #if UNITY_EDITOR
            if (soundPrefab == null) { Debug.LogWarning("SoundPrefab is null in AudioSettings"); }
        #endif
    }

    public void OnFixedUpdate(){
        foreach (Sound currentSound in sounds){
            currentSound.OnFixedUpdate();
        }
    }

    public LoopingSound AddLoopingSound(AK.Wwise.Event startEvent, AK.Wwise.Event stopEvent, Vector3 pos){
        GameObject soundGameObject = GameObject.Instantiate(soundPrefab, pos, Quaternion.identity);
        soundGameObject.transform.SetParent(GameManager.Instance.transform);

        LoopingSound newLoopingSound = new LoopingSound(this, soundGameObject, startEvent, stopEvent);
        loopingSounds.Add(newLoopingSound);
        return newLoopingSound;
    }

    public Sound AddSound(AK.Wwise.Event startEvent, float existLenght, Vector3 pos){
        GameObject soundGameObject = GameObject.Instantiate(soundPrefab, pos, Quaternion.identity);
        soundGameObject.transform.SetParent(GameManager.Instance.transform);

        Sound newSound = new Sound(this, soundGameObject, startEvent, existLenght);
        sounds.Add(newSound);
        return newSound;
    }

    public void StopSound(Sound sound){
        GameObject.Destroy(sound.SoundGameObject);
        sounds.Remove(sound);
    }

    public void StopLoopingSound(LoopingSound sound){
        GameObject.Destroy(sound.soundGameObject);
        loopingSounds.Remove(sound);
    }
}
