using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class JoystickController : IController
{
     XboxController controller;

    public JoystickController(ControllerType _controllerType) : base(_controllerType)
    {
        switch (_controllerType)
        {
            case ControllerType.Xbox_First:
                controller = XboxController.First;
                break;
            case ControllerType.Xbox_Second:
                controller = XboxController.Second;
                break;
            case ControllerType.Xbox_Third:
                controller = XboxController.Third;
                break;
            case ControllerType.Xbox_Fourth:
                controller = XboxController.Fourth;
                break;
        }
    }

    public override bool PressButtonA()
    {
        return XCI.GetButtonDown(XboxButton.B, controller);
    }

    public override bool PressButtonB()
    {
        return XCI.GetButtonDown(XboxButton.A, controller);
    }

    public override float Horizontal()
    {
        return XCI.GetAxis(XboxAxis.LeftStickX, controller);
    }

    public override float Vertical()
    {
        return XCI.GetAxis(XboxAxis.LeftStickY, controller);
    }
}
