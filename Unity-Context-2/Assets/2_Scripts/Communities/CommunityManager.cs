using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityManager
{
    private Dictionary<Communities, Community> communities = new Dictionary<Communities, Community>();

    //-------------------------------------------

    public CommunityManager(){

        Add(Communities.circle);
        Add(Communities.triangle);
        Add(Communities.square);
    }

    //----------------------------------------------

    private void Add(Communities community){
        communities.Add(community, new Community(community));
    }
}
