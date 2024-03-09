using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    //referance to the renderer that holds the material instance, editing the material directly would edit it for every object
    private new MeshRenderer renderer;

    [SerializeField]
    private Color emissionColor;
    [SerializeField]
    private bool hasEmission;
    private bool oldBool;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        ChangeEmissionBool();
        ChangeEmissionColor();
    }

    private void ChangeEmissionColor()
    {
        if (!hasEmission) { return; }
        renderer.material.SetColor("_EmissionColor", emissionColor);
    }

    private void ChangeEmissionBool()
    {
        if(hasEmission == oldBool) { return; }

        if (hasEmission)
        {
            oldBool = hasEmission;
            renderer.material.EnableKeyword("_EMISSION");
        }
        else
        {
            oldBool = hasEmission;
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}
