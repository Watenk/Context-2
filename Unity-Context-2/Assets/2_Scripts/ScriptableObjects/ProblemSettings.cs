using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ProblemSettings", menuName = "ScriptableObjects/ProblemSettings")]
public class ProblemSettings : ScriptableObject
{
    public static ProblemSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<ProblemSettings>("ProblemSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("ProblemSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static ProblemSettings instance;

    //-----------------------------------------------

    [Tooltip("The range in which agents and the player are detected around a problem")]
    public float ProblemDetectRange;
}
