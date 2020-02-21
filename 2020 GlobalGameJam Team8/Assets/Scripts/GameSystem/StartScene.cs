using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.First)||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Second)||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Fourth)||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Third) ||
            Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Keypad1))
            SceneManager.LoadScene("PlayerMenu");
    }
}
