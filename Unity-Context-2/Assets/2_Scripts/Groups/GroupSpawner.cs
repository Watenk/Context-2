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
    public float WanderFromHomeDistance;
    public bool IsActive;

    private Group group;

    //--------------------------------------------------------
    
    public void Start(){
        group = GameManager.GetService<CommunityManager>().AddGroup(CommunityType, GroupSize, WanderFromHomeDistance, transform.position, SpawnRadius, IsActive);        
    }

    public Group GetGroup(){

        #if UNITY_EDITOR
            if (group == null) { Debug.LogError("Tried to get group before its initialized"); }
        #endif

        GameObject.Destroy(this.gameObject);
        return group;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsActive ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 2);
        Gizmos.DrawWireSphere(transform.position, WanderFromHomeDistance);
    }
}
