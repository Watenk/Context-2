using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class BubbleController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;
    private VisualEffect activeEffect;


    [SerializeField]
    private float growSpeed = 0.075f;
    [SerializeField]
    private float radius = 2;


    private void Start()
    {
        visualEffect.SetFloat("GrowSpeed", growSpeed);
        visualEffect.SetFloat("Radius", radius);
    }

    public void StartBubble(ChimeInputs chimeInput)
    {
        if (activeEffect != null)
        {
            activeEffect.SetBool("ActiveBool", false);
            Destroy(activeEffect.gameObject, 1);
        }
        activeEffect = Instantiate(visualEffect, visualEffect.transform.parent);

        switch (chimeInput)
        {
            case ChimeInputs.circle:
                activeEffect.SetVector4("Color", Color.yellow);
                break;
            case ChimeInputs.square:
                activeEffect.SetVector4("Color", Color.blue);
                break;
            case ChimeInputs.triangle:
                activeEffect.SetVector4("Color", Color.red);
                break;
        }
        activeEffect.Play();
        activeEffect.SetBool("ActiveBool", true);
    }

    public void StartBubble(CommunityTypes community, float duration)
    {
        if (activeEffect != null)
        {
            activeEffect.SetBool("ActiveBool", false);
            Destroy(activeEffect.gameObject, 1);
        }
        activeEffect = Instantiate(visualEffect, visualEffect.transform.parent);

        switch (community)
        {
            case CommunityTypes.circle:
                activeEffect.SetVector4("Color", Color.yellow);
                break;
            case CommunityTypes.square:
                activeEffect.SetVector4("Color", Color.blue);
                break;
            case CommunityTypes.triangle:
                activeEffect.SetVector4("Color", Color.red);
                break;
        }
        activeEffect.Play();
        activeEffect.SetBool("ActiveBool", true);
        StartCoroutine(StopAfterTime(duration));
    }


    public void StopBubble()
    {
        if (activeEffect != null)
        {
            activeEffect.SetBool("ActiveBool", false);
            Destroy(activeEffect.gameObject, 1);
        }
    }


    private IEnumerator StopAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopBubble();
    }
}
