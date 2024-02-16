using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroupManager : IFixedUpdateable
{
    private Dictionary<Groups, Group> groupsDict = new Dictionary<Groups, Group>();

    //References
    private GameManager gameManager;

    //------------------------------------------------

    public GroupManager(){
        gameManager = GameManager.Instance;
    }

    public void OnFixedUpdate(){

    }

    public void AddGroup(Groups group, Vector3 pos){
        groupsDict.Add(group, new Group());
    }
}
