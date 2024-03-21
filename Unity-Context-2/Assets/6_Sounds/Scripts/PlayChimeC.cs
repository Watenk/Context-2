using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayChimeTestC : MonoBehaviour
{
    public AK.Wwise.Event SomeSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SomeSound.Post(gameObject);
        }
    }
}
