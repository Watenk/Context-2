using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IUpdateable
{
    public event Action<Vector2> OnPlayerMove;

    //---------------------------------------------------------

    public void OnUpdate(){
        OnPlayerMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    //---------------------------------------------------------
}
