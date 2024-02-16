using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;

    private Rigidbody rb;
    private float speed;
    private float rotationSpeed;

    // References
    private InputManager inputManager;

    //------------------------------------------------

    public void Start(){
        inputManager = GameManager.Instance.GetService<InputManager>();
        rb = GetComponent<Rigidbody>();
        speed = PlayerSettings.Instance.Speed;
        rotationSpeed = PlayerSettings.Instance.RotationSpeed;

        #if UNITY_EDITOR
            if (rb == null) { Debug.LogError("Player doesnt contain a rigidbody"); }
        #endif

        inputManager.OnPlayerMove += OnPlayerMove;
    }

    //-----------------------------------------------

    private void OnPlayerMove(Vector2 playerMovement){

        // Move Player
        rb.AddForce(new Vector3(playerMovement.x, 0, playerMovement.y).normalized * speed * Time.deltaTime, ForceMode.VelocityChange);

        // Rotate body
        if (playerMovement.x != 0 || playerMovement.y != 0){
            float bodyYRotation = Mathf.Atan2(playerMovement.x, playerMovement.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, bodyYRotation, 0);
            Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}