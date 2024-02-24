using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : IUpdateable
{
    private List<GameObject> sounds = new List<GameObject>();

    // References
    private InputManager inputManager;
    private PlayerController player;

    //-----------------------------------------

    public AudioManager(){
        inputManager = GameManager.Instance.GetService<InputManager>();
        inputManager.OnSquareDown += OnSquareDown;
        inputManager.OnCircleDown += OnCircleDown;
        inputManager.OnTriangleDown += OnTriangleDown;
        inputManager.OnCrossDown += OnCrossDown;
        inputManager.OnSquareUp += OnSquareUp;
        inputManager.OnCircleUp += OnCircleUp;
        inputManager.OnTriangleUp += OnTriangleUp;
        inputManager.OnCrossUp += OnCrossUp;
        player = GameManager.Instance.Player;
    }

    public void OnUpdate(){

    }

    public void PlaySoundFor(float lenght){

    }

    //-----------------------------------

    private void InstanceSound(AK.Wwise.Event wwiseEvent, Vector3 pos){
        GameObject soundInstance = GameObject.Instantiate(new GameObject(), pos, Quaternion.identity);
        soundInstance.transform.SetParent(GameManager.Instance.transform);
        sounds.Add(soundInstance);
        wwiseEvent.Post(soundInstance);
    }

    private void OnSquareDown(){
        InstanceSound(AudioSettings.Instance.PlayPlayerSquareChime, player.transform.position);
    }

    private void OnCircleDown(){
        InstanceSound(AudioSettings.Instance.PlayPlayerCircleChime, player.transform.position);
    }

    private void OnTriangleDown(){
        InstanceSound(AudioSettings.Instance.PlayPlayerTriangleChime, player.transform.position);
    }

    private void OnCrossDown(){
    }

    private void OnSquareUp(){
        InstanceSound(AudioSettings.Instance.StopPlayerSquareChime, player.transform.position);
    }

    private void OnCircleUp(){
        InstanceSound(AudioSettings.Instance.StopPlayerCircleChime, player.transform.position);
    }

    private void OnTriangleUp(){
        InstanceSound(AudioSettings.Instance.StopPlayerTriangleChime, player.transform.position);
    }

    private void OnCrossUp(){
    }
}
