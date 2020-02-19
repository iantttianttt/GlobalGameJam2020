using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IController
{

    public ControllerType controllerType;


    public IController( ControllerType _controllerType)
    {
        controllerType = _controllerType;
    }

    /// <summary>
    /// 按鍵A觸發
    /// </summary>
    /// <returns></returns>
    public abstract bool PressButtonA();
    /// <summary>
    /// 按鍵B觸發
    /// </summary>
    /// <returns></returns>
    public abstract bool PressButtonB();
    /// <summary>
    /// 移動水平軸
    /// </summary>
    /// <returns></returns>
    public abstract float Horizontal();
    /// <summary>
    /// 移動垂直軸
    /// </summary>
    /// <returns></returns>
    public abstract float Vertical();


}
