using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MuralSettings", menuName = "ScriptableObjects/MuralSettings")]
public class MuralSettings : ScriptableObject
{
    public static MuralSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<MuralSettings>("MuralSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("MuralSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static MuralSettings instance;

    //--------------------------------------------

    [Tooltip("Range the player must be in for the mural to activate")]
    public float DetectRange;
    [Tooltip("Time in seconds between each played short note / chime")]
    public float ShortChimePlayDelay;
    [Tooltip("Time in seconds between each played long note / chime")]
    public float LongChimePlayDelay;
    [Tooltip("Distance between the mushrooms")]
    public float MushroomDistance;
    public List<MuralMushroomPrefab> MushroomPrefabs;
}
