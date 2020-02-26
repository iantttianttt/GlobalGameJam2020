using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_GameplayMain : IUIPanel
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------
    public Image Bar;
    public Image Pointer;

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Consturcter
    //-----------------------------------------------------------------------
    public UIPanel_GameplayMain()
	{
		mUIPanelType = EUIPanelType.GAMEPLAY_MAIN;
	}

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 設定壓力計
    /// </summary>
    /// <param name="pressure"></param>
    public void SetBar(float pressure, float maxPressure)
    {
        Bar.fillAmount = pressure / maxPressure;
        Pointer.rectTransform.eulerAngles = new Vector3(0, 0, pressure / maxPressure * -140);
    }


}
