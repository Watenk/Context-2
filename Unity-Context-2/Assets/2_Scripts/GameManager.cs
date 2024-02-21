using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerController Player { get; private set; }

    private Dictionary<Type, System.Object> services = new Dictionary<Type, System.Object>();
    private List<IUpdateable> updateables = new List<IUpdateable>();
    private List<IFixedUpdateable> fixedUpdateables = new List<IFixedUpdateable>();

    //------------------------------------------------------------

    public void Awake(){
        Instance = this;

        AddService(new InputManager());
        AddService(new GroupsManager());
        AddService(new TimerManager());
    }

    public void Update(){
        foreach (IUpdateable current in updateables) { current.OnUpdate(); }
    }

    public void FixedUpdate(){
        foreach (IFixedUpdateable current in fixedUpdateables) { current.OnFixedUpdate(); }
    }

    public T GetService<T>(){
        services.TryGetValue(typeof(T), out System.Object service);

        #if UNITY_EDITOR
            if (service == null) { Debug.LogError("Tried to get service " + typeof(T).Name + " but it doesnt exist"); }
        #endif

        return (T)service;
    }

    public void SetPlayer(PlayerController player){
        Player = player;
    }

    //-------------------------------------------------------------

    private void AddService<T>(T service){
        services.Add(typeof(T), service);

        if (service is IUpdateable) { updateables.Add((IUpdateable)service); }
        if (service is IFixedUpdateable) { fixedUpdateables.Add((IFixedUpdateable)service); }
    }
}
