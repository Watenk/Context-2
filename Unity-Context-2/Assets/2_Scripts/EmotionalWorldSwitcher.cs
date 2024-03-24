using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionalWorldSwitcher : MonoBehaviour
{
    [SerializeField]
    private float timeNeeded;
    
    [SerializeField]
    private LayerMask normalLayers;

    [SerializeField]
    private LayerMask EmotionalLayer;

    [SerializeField]
    private Image overlayImage;

    [SerializeField]
    private Rigidbody player;

    [SerializeField]
    private GameObject lightObject;

    private new Camera camera;


    private bool state;
    private float timeNotMoved;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.velocity.magnitude < 0.1f) 
        {
            if (timeNotMoved < timeNeeded)
            {
                timeNotMoved += Time.deltaTime;
            }
        }
        else
        {
            if(timeNotMoved > 0)
            {
                timeNotMoved -= Time.deltaTime;
            }
        }

        overlayImage.color = new Color(0, 0, 0, Mathf.InverseLerp(1, timeNeeded, timeNotMoved));


        if (state)
        {
            if (timeNotMoved < timeNeeded)
            {
                SwitchState();
                overlayImage.enabled = true;
            }
        }
        else
        {
            if (timeNotMoved >= timeNeeded)
            {
                SwitchState();
                overlayImage.enabled = false;
            }
        }
    }





    private void SwitchState()
    {
        state = !state;

        if (state)
        {
            camera.cullingMask = EmotionalLayer;
            RenderColor(Color.black, camera);
            lightObject.SetActive(false);
        }
        else
        {
            camera.cullingMask = normalLayers;
            RenderSkybox(camera);
            lightObject.SetActive(true);
        }


    }
    public void RenderSkybox(Camera targetCamera = null)
    {
        if (targetCamera == null)
        {
            //Get reference to main camera if no camera is passed
            targetCamera = Camera.main;
        }
        //set camera to render the skybox
        targetCamera.clearFlags = CameraClearFlags.Skybox;
    }

    public void RenderColor(Color color, Camera targetCamera = null)
    {
        if (targetCamera == null)
        {
            //Get reference to main camera if no camera is passed
            targetCamera = Camera.main;
        }

        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = color;
    }
}
