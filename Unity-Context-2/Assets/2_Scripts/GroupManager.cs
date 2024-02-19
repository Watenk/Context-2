using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : IFixedUpdateable
{
    private List<Group> groups = new List<Group>();

    //References
    private GameManager gameManager;

    //------------------------------------------------

    public GroupManager(){
        gameManager = GameManager.Instance;
    }

    public void OnFixedUpdate(){

    }

    public void AddGroup(AgentType agentType, int size, Vector3 homePos){
        groups.Add(new Group(agentType, homePos));
    }
}
