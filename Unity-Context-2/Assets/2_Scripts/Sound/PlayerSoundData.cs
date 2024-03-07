using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSoundData
{
    [Tooltip("This name has no function other than organizing the scriptableObject")]
    public string Name;
    public ChimeInputs ChimeInput;
    public AK.Wwise.Event StartWwiseEvent;
    public AK.Wwise.Event StopWwiseEvent;
}
