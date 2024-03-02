using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    public AgentType AgentType;
    public int GroupSize;
    public float SpawnRadius; [Tooltip("Range around the GroupSpawner agents will spawn in")]

    //--------------------------------------------------------
    
    void Start(){
        //GameManager.GetService<GroupsManager>().AddGroup(AgentType, GroupSize, transform.position, SpawnRadius);        
        //GameObject.Destroy(this.gameObject);
    }
}
