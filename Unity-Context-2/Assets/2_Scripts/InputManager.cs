using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IUpdateable
{
    public event Action<Vector2> OnPlayerMove;
    public event Action OnSpace;
    public event Action OnSquareDown;
    public event Action OnTriangleDown;
    public event Action OnCircleDown;
    public event Action OnCrossDown;
    public event Action OnSquareUp;
    public event Action OnTriangleUp;
    public event Action OnCircleUp;
    public event Action OnCrossUp;

    private bool square;
    private bool triangle;
    private bool circle;
    private bool cross;

    //---------------------------------------------------------

    public void OnUpdate(){
        OnPlayerMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        if (Input.GetKey(KeyCode.Space) && OnSpace != null) { OnSpace(); }
        if (Input.GetAxis("Square") > 0 && !square && OnSquareDown != null) { OnSquareDown(); square = true; }
        if (Input.GetAxis("Triangle") > 0 && !triangle && OnTriangleDown != null) { OnTriangleDown(); triangle = true; }
        if (Input.GetAxis("Circle") > 0 && !circle && OnCircleDown != null) { OnCircleDown(); circle = true; }
        if (Input.GetAxis("Cross") > 0 && !cross && OnCrossDown != null) { OnCrossDown(); cross = true; }
        if (Input.GetAxis("Square") == 0 && square && OnSquareUp != null) { OnSquareUp(); square = false; }
        if (Input.GetAxis("Triangle") == 0 && triangle && OnTriangleUp != null) { OnTriangleUp(); triangle = false; }
        if (Input.GetAxis("Circle") == 0 && circle && OnCircleUp != null) { OnCircleUp(); circle = false; }
        if (Input.GetAxis("Cross") == 0 && cross && OnCrossUp != null) { OnCrossUp(); cross = false; }
    }

    //---------------------------------------------------------
}
