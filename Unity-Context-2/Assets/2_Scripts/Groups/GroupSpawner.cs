using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    public CommunityTypes CommunityType;
    public int GroupSize;
    [Tooltip("Range around the GroupSpawner agents will spawn in")]
    public float SpawnRadius; 
    [Tooltip("Is the group availible to command or is the group inactive")]
    public bool IsActive;

    //--------------------------------------------------------
    
    void Start(){
        GameManager.GetService<CommunityManager>().AddGroup(CommunityType, GroupSize, transform.position, SpawnRadius, IsActive);        
        GameObject.Destroy(this.gameObject);
    }
}
