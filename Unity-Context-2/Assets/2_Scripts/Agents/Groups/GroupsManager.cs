using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupsManager 
{
    private List<Group> groups = new List<Group>();

    //References
    private GameManager gameManager;

    //------------------------------------------------

    public GroupsManager(){
        gameManager = GameManager.Instance;
    }

    public void AddGroup(AgentType agentType, int groupSize, Vector3 homePos, float spawnRadius){
        groups.Add(new Group(agentType, groupSize, homePos, spawnRadius));
    }

    public void RemoveGroup(Group group){
        group.DestroyAgents();
        groups.Remove(group);
    }
}
