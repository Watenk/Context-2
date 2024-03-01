using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChimeX : MonoBehaviour
{
    public AK.Wwise.Event SomeSound1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.X))
        {
            SomeSound1.Post(gameObject);
        }
    }
}
