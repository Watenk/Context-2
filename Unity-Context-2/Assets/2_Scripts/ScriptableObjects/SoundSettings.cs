using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "ScriptableObjects/SoundSettings")]
public class SoundSettings : ScriptableObject
{
    public static SoundSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<SoundSettings>("SoundSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("SoundSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static SoundSettings instance;

    //--------------------------------------------

    public GameObject SoundPrefab;
    public float SoundDestroyTime;

    public List<PlayerSoundData> PlayerChimes;
    public List<NPCSoundData> NPCChimes;
}
