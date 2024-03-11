using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;

    private Rigidbody rb;
    private float speed;
    private float rotationSpeed;
    private Queue<LoopingSound> activeSounds = new Queue<LoopingSound>();

    // References
    private InputHandler inputManager;
    private SoundManager soundManager;
    [SerializeField]
    private VisualEffect visualEffect;

    //------------------------------------------------

    public void Awake(){
        rb = GetComponent<Rigidbody>();

        #if UNITY_EDITOR
            if (rb == null) { Debug.LogError("Player doesnt contain a rigidbody"); }
        #endif
    }

    public void Start(){
        inputManager = GameManager.GetService<InputHandler>();
        soundManager = GameManager.GetService<SoundManager>();
        speed = PlayerSettings.Instance.Speed;
        rotationSpeed = PlayerSettings.Instance.RotationSpeed;

        inputManager.OnPlayerMove += OnPlayerMove;
        inputManager.OnChimeDown += OnChimeDown;
        inputManager.OnChimeUp += OnChimeUp;
    }

    //-----------------------------------------------

    private void OnPlayerMove(Vector2 playerMovement){

        Vector3 playerDirection = new Vector3(playerMovement.x, 0, playerMovement.y).normalized;
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 dir = Quaternion.LookRotation(camForward) * playerDirection;

        rb.AddForce(dir * speed * Time.deltaTime, ForceMode.VelocityChange);

        // Rotate body
        if (playerMovement.x != 0 || playerMovement.y != 0){
            float bodyYRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Quaternion.Euler(0, bodyYRotation, 0), rotationSpeed * Time.deltaTime);
        }
    }

    private void OnChimeDown(ChimeInputs chimeInput){
        PlayerSoundData soundData = soundManager.GetPlayerSound(chimeInput);
        LoopingSound sound = soundManager.PlayLoopingSound(soundData, gameObject.transform.position);
        activeSounds.Enqueue(sound);
        visualEffect.Play();
        visualEffect.SetBool("ActiveBool", true);
    }

    private void OnChimeUp(ChimeInputs chimeInput){
        if (activeSounds.Count == 0) { return; }

        LoopingSound sound = activeSounds.Dequeue();
        sound.StopSound();
        visualEffect.SetBool("ActiveBool", false);
    }
}
