using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;
    public CommunityTypes CurrentCommunity; //{ get; private set; }

    private Rigidbody rb;
    private float speed;
    private float rotationSpeed;
    private Queue<LoopingSound> activeSounds = new Queue<LoopingSound>();
    private Animator animator;

    // References
    private InputHandler inputManager;
    private SoundManager soundManager;
    [SerializeField]
    private BubbleController bubbleController;

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
        animator = GetComponentInChildren<Animator>();

        #if UNITY_EDITOR
            if (animator == null) Debug.LogError("Player doesn't contain an animator");
        #endif

        inputManager.OnPlayerMove += OnPlayerMove;
        inputManager.OnChimeDown += OnChimeDown;
        inputManager.OnChimeUp += OnChimeUp;
    }

    void Update(){
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Mult", rb.velocity.magnitude/ 3.8f);
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer("TriangleCommunity")){
            CurrentCommunity = CommunityTypes.triangle;
            AkSoundEngine.SetState(3607165242U, 438105790U);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("SquareCommunity")){
            CurrentCommunity = CommunityTypes.square;
            AkSoundEngine.SetState(3607165242U, 438105790U);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("CircleCommunity")){
            CurrentCommunity = CommunityTypes.circle;
            AkSoundEngine.SetState(3607165242U, 438105790U);
        }
    }

    void OnTriggerExit(){
        CurrentCommunity = CommunityTypes.global;
        AkSoundEngine.SetState(3607165242U, 3553349781U);
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
        LoopingSound sound = soundManager.PlayPlayerSound(soundData, Camera.main.transform.position);
        activeSounds.Enqueue(sound);
        bubbleController.StartBubble(chimeInput);
    }

    private void OnChimeUp(ChimeInputs chimeInput){
        if (activeSounds.Count == 0) { return; }

        LoopingSound sound = activeSounds.Dequeue();
        sound.StopSound();
        bubbleController.StopBubble();
    }
}
