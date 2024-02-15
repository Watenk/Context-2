using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;

    // References
    private InputManager inputManager;

    //------------------------------------------------

    public void Start(){
        rb = GetComponent<Rigidbody>();
        speed = PlayerSettings.Instance.Speed;
        inputManager = GameManager.Instance.GetService<InputManager>();

        #if UNITY_EDITOR
            if (rb == null) { Debug.LogError("Player doesnt contain a rigidbody"); }
        #endif

        inputManager.OnW += OnW;
        inputManager.OnA += OnA;
        inputManager.OnS += OnS;
        inputManager.OnD += OnD;
    }

    public void OnW(){
        rb.AddForce(Vector3.forward * 10 * speed);
    }

    public void OnA(){
        rb.AddForce(Vector3.forward * 10 * speed);
    }

    public void OnS(){
        rb.AddForce(Vector3.forward * 10 * speed);
    }

    public void OnD(){
        rb.AddForce(Vector3.forward * 10 * speed);
    }
}