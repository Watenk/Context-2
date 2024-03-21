using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chime
{
    public ChimeInputs chimeInput;
    public bool isLong;

    //---------------------------------------

    public Chime(ChimeInputs chimeInput, bool isLong){
        this.chimeInput = chimeInput;
        this.isLong = isLong;
    }
}
