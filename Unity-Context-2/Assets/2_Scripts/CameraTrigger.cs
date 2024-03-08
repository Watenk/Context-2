using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;


    private void OnTriggerEnter(Collider other)
    {
        virtualCamera.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        virtualCamera.SetActive(false);
    }


}
