using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChimeSettings", menuName = "ScriptableObjects/ChimeSettings")]
public class ChimeSettings : ScriptableObject
{
    public static ChimeSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<ChimeSettings>("ChimeSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("ChimeSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static ChimeSettings instance;

    //--------------------------------------------

    [Tooltip("Tile in seconds it takes for a chime chain to reset")]
    public float ChimeResetTime;
    [Tooltip("How long a button needs to be hold for it to be a long chime")]
    public float LongChimeLenght;
    public List<ChimeSequence> ChimeSequences = new List<ChimeSequence>();
}
