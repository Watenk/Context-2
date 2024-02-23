using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentPrefab 
{
    public AgentType agentType;
    public List<GameObject> prefabs = new List<GameObject>();
}
