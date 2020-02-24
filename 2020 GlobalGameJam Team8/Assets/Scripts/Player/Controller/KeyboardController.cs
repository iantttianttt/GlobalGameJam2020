using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : IController
{

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
        float AnalogX ;
        float AnalogY ;

        if (controllerType == ControllerType.Keyboard1)
        {
            AnalogX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            AnalogY = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else
        {
            AnalogX = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            AnalogY = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
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

