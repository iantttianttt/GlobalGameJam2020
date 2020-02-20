using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : IController
{
    private float AnalogX = 0f;
    private float AnalogY = 0f;

    public KeyboardController(ControllerType _controllerType) : base(_controllerType)
    {

    }

    public override bool PressButtonA()
    {
        return controllerType == ControllerType.Keyboard1 ? Input.GetKeyDown(KeyCode.G) : Input.GetKeyDown(KeyCode.Keypad1);
    }

    public override bool PressButtonB()
    {
        return controllerType == ControllerType.Keyboard1 ? Input.GetKeyDown(KeyCode.H) : Input.GetKeyDown(KeyCode.Keypad2);
    }

    public Vector2 AnalogSimulation()
    {
        if (controllerType == ControllerType.Keyboard1)
        {
            if (Input.GetKey(KeyCode.D))
                AnalogX = 1;
            else if (Input.GetKey(KeyCode.A))
                AnalogX = -1;
            else AnalogX = 0;

            if (Input.GetKey(KeyCode.W))
                AnalogY = 1;
            else if (Input.GetKey(KeyCode.S))
                AnalogY = -1;
            else AnalogY = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                AnalogX = 1;
            else if (Input.GetKey(KeyCode.LeftArrow))
                AnalogX = -1;
            else AnalogX = 0;

            if (Input.GetKey(KeyCode.UpArrow))
                AnalogY = 1;
            else if (Input.GetKey(KeyCode.DownArrow))
                AnalogY = -1;
            else AnalogY = 0;
        }

        return new Vector2(Mathf.Cos(Mathf.Atan2(AnalogY, AnalogX)), Mathf.Sin(Mathf.Atan2(AnalogY, AnalogX)))*(Mathf.Abs(AnalogX)+ Mathf.Abs(AnalogY) > 0 ? 1: 0);
    }

    public override float Horizontal()
    {
        return AnalogSimulation().x;
    }

    public override float Vertical()
    {
        return AnalogSimulation().y;
    }
}

