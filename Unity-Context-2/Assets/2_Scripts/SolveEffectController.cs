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
            mesh.gameObject.SetActive(false);
        }
        foreach (SkinnedMeshRenderer mesh in skinnedMeshRenderers)
        {
            mesh.material.SetFloat("_CutOffHeight", cutOffHeight);
            mesh.gameObject.SetActive(false);
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
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.gameObject.SetActive(true);
        }
        foreach (SkinnedMeshRenderer mesh in skinnedMeshRenderers)
        {
            mesh.gameObject.SetActive(true);
        }
        while (cutOffHeight < 60)
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
