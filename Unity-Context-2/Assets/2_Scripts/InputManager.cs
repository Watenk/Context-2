using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IUpdateable
{
    public event Action OnW;
    public event Action OnA;
    public event Action OnS;
    public event Action OnD;

    //---------------------------------------------------------

    public void OnUpdate(){
        CheckKey(KeyCode.W, OnW);
        CheckKey(KeyCode.A, OnA);
        CheckKey(KeyCode.S, OnS);
        CheckKey(KeyCode.D, OnD);
    }

    //---------------------------------------------------------

    private void CheckKeyDown(KeyCode keyCode, Action action){
        if (Input.GetKeyDown(keyCode) && action != null){
            action.Invoke();
        }
    }

    private void CheckKeyDown<T>(KeyCode keyCode, Action<T> action, T data){
        if (Input.GetKeyDown(keyCode) && action != null){
            action.Invoke(data);
        }
    }

    private void CheckKey(KeyCode keyCode, Action action){
        if (Input.GetKey(keyCode) && action != null){
            action.Invoke();
        }
    }

    private void CheckKey<T>(KeyCode keyCode, Action<T> action, T data){
        if (Input.GetKey(keyCode) && action != null){
            action.Invoke(data);
        }
    }
}
