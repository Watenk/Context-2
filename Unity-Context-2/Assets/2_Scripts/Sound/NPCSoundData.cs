using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCSoundData
{
    [Tooltip("This name has no function other than organizing the scriptableObject")]
    public string Name;
    public ChimeTasks ChimeTask;
    public AK.Wwise.Event WwiseEvent;
}