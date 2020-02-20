using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDisplay :MonoBehaviour
{
    public GameObject Red;
    public GameObject Blue;
   
    public void CanNotPut()
    {
        Red.SetActive(true);
        Blue.SetActive(false);
    }

    public void CanPut()
    {
        Red.SetActive(false);
        Blue.SetActive(true);
    }

    public void DisablePutDisplay()
    {
        Red.SetActive(false);
        Blue.SetActive(false);
        gameObject.SetActive(false);
    }
}
