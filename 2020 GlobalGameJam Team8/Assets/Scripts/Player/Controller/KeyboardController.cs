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

    public override float Horizontal()
    {
        if (controllerType == ControllerType.Keyboard1)
        {
            if (Input.GetKey(KeyCode.D))
                return 1;
            else if (Input.GetKey(KeyCode.A))
                return -1;
            else return 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                return 1;
            else if (Input.GetKey(KeyCode.LeftArrow))
                return -1;
            else return 0;
        }
      
    }

    public override float Vertical()
    {
        if (controllerType == ControllerType.Keyboard1)
        {
            if (Input.GetKey(KeyCode.W))
                return 1;
            else if (Input.GetKey(KeyCode.S))
                return -1;
            else return 0;
        }
        else{
            if (Input.GetKey(KeyCode.UpArrow))
                return 1;
            else if (Input.GetKey(KeyCode.DownArrow))
                return -1;
            else return 0;
        }
    }
}

