using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group
{
    public AgentType agentType { get; private set; }
    public Vector3 Home { get; private set; }

    private List<Agent> agents = new List<Agent>();

    //-----------------------------------------------

    public Group(AgentType agentType, Vector3 home){
        this.agentType = agentType;
        this.Home = home;
    }
}
