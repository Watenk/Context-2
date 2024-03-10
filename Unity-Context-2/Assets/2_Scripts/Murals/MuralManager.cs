using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralManager : IFixedUpdateable
{
    private List<Mural> murals = new List<Mural>();

    //----------------------------------------------

    public void OnFixedUpdate(){
        foreach (Mural mural in murals){
            mural.OnFixedUpdate();
        }
    }

    public void Add(Mural mural){
        murals.Add(mural);
    }
}