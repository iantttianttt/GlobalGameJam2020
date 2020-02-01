﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Image Bar;
    public Image Pointer;
    void Start()
    {
        
    }

    void Update()
    {
    }

    /// <summary>
    /// 設定壓力計
    /// </summary>
    /// <param name="pressure"></param>
    public void SetBar(float pressure)
    {
        Bar.fillAmount = pressure;
        Pointer.rectTransform.eulerAngles=new Vector3(0, 0, pressure*-140);
    }
}
