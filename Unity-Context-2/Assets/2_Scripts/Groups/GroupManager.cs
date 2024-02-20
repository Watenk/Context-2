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

    public void AddGroup(AgentType agentType, int groupSize, Vector3 homePos, float spawnRadius){
        groups.Add(new Group(agentType, groupSize, homePos, spawnRadius, this));
    }

    public void RemoveGroup(Group group){
        group.DestroyAgents();
        groups.Remove(group);
    }
}
