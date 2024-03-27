using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorController : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        animator.SetBool("Active", true);
    }
}
