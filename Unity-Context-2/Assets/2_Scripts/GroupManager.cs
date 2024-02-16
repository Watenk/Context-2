using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroupManager : IFixedUpdateable
{
    private Dictionary<AgentType, Group> groupsDict = new Dictionary<AgentType, Group>();

    //References
    private GameManager gameManager;

    //------------------------------------------------

    public GroupManager(){
        gameManager = GameManager.Instance;
    }

    public void OnFixedUpdate(){

    }

    public void AddGroup(AgentType group, Vector3 pos){
        groupsDict.Add(group, new Group());
    }
}
