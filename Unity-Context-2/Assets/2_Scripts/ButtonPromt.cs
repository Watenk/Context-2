using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPromt : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void Trigger()
    {
        if(animator != null)
        {
            animator.SetTrigger("Activate");
        }
    }
}
