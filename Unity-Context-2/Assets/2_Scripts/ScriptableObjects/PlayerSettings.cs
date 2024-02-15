using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public static PlayerSettings Instance { 
        get{
            if (instance == null){
                instance = Resources.Load<PlayerSettings>("PlayerSettings");

                #if UNITY_EDITOR
                    if (instance == null) {Debug.LogError("PlayerSettings couldn't be loaded...");}
                #endif
            }
            return instance;
        }
    }
    private static PlayerSettings instance;

    //------------------------------------------------------------------

    public float Speed;
}
