using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Image Bar;
    public Image Pointer;
    public GameObject GameOver;
    public GameObject YouWin;

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
    public void SetBar(float pressure, float maxPressure)
    {
        Bar.fillAmount = pressure/ maxPressure;
        Pointer.rectTransform.eulerAngles=new Vector3(0, 0, pressure / maxPressure * -140);
    }

    public void Win()
    {
        YouWin.SetActive(true);
    }

    public void Lose()
    {
        GameOver.SetActive(true);
    }
}
