using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveEffectController : MonoBehaviour
{
    [SerializeField]
    private float cutOffHeight;

    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;



    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.material.SetFloat("_CutOffHeight", cutOffHeight);
        }
        foreach (SkinnedMeshRenderer mesh in skinnedMeshRenderers)
        {
            mesh.material.SetFloat("_CutOffHeight", cutOffHeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while(cutOffHeight < 60)
        {
            cutOffHeight += Time.deltaTime;
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.material.SetFloat("_CutOffHeight", cutOffHeight);
            }
            foreach (SkinnedMeshRenderer mesh in skinnedMeshRenderers)
            {
                mesh.material.SetFloat("_CutOffHeight", cutOffHeight);
            }
            yield return null;
        }
    }
}
