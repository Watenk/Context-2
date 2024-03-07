using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "ScriptableObjects/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public static AudioSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<AudioSettings>("AudioSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("AudioSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static AudioSettings instance;

    //--------------------------------------------

    public GameObject SoundPrefab;
    public float SoundDestroyTime;

    [Header("PlayerChimes")]
    public AK.Wwise.Event PlayPlayerCircleChime;
    public AK.Wwise.Event PlayPlayerTriangleChime;
    public AK.Wwise.Event PlayPlayerSquareChime;
    public AK.Wwise.Event PlayPlayerGlobalChime;
    public AK.Wwise.Event StopPlayerCircleChime;
    public AK.Wwise.Event StopPlayerTriangleChime;
    public AK.Wwise.Event StopPlayerSquareChime;
    public AK.Wwise.Event StopPlayerGlobalChime;

    [Header("NPCChimes")]
    public AK.Wwise.Event ShortNPCCircleChime;
    public AK.Wwise.Event ShortNPCTriangleChime;
    public AK.Wwise.Event ShortNPCSquareChime;
    public AK.Wwise.Event ShortNPCGlobalChime;
    public AK.Wwise.Event LongNPCCircleChime;
    public AK.Wwise.Event LongNPCTriangleChime;
    public AK.Wwise.Event LongNPCSquareChime;
    public AK.Wwise.Event LongNPCGlobalChime;
}
