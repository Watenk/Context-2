using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChimeSequence
{
    [Tooltip("This name has no function, Its just to organize the scriptable object")]
    public string Name;
    [Tooltip("The task that is executed after the chime is entered")]
    public ChimeTasks chimeTask;
    [Tooltip("Chimes in the sequence")]
    public List<Chime> chimes = new List<Chime>();
    [Tooltip("What communities are affected by this chime")]
    public List<Communities> affectedCommunities;
}
