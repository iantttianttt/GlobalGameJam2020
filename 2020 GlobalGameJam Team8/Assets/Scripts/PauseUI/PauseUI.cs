using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class PauseUI : MonoBehaviour
{
    public Image ContinueImage;
    public Image RestartImage;
    public Image ExitImage;

    public Sprite ContinueSprite;
    public Sprite ContinueSprite_Gray;

    public Sprite RestartSprite;
    public Sprite RestartSprite_Gray;

    public Sprite ExitSprite;
    public Sprite ExitSprite_Gray;

    int selectID=0;

    void Update()
    {
        if(XCI.GetButtonDown(XboxButton.DPadUp, XboxController.All)){
            selectID--;
        }

        if (XCI.GetButtonDown(XboxButton.DPadDown, XboxController.All)){
            selectID++;
        }

        if (selectID < 0)
            selectID = 2;

        if (selectID > 2)
            selectID = 0;

        switch (selectID) {
            case 0:
                ContinueImage.sprite = ContinueSprite_Gray;
                RestartImage.sprite = RestartSprite;
                ExitImage.sprite = ExitSprite;

                if (XCI.GetButtonDown(XboxButton.A, XboxController.All))
                    Continue();

                break;

            case 1:
                ContinueImage.sprite = ContinueSprite;
                RestartImage.sprite = RestartSprite_Gray;
                ExitImage.sprite = ExitSprite;

                if (XCI.GetButtonDown(XboxButton.A, XboxController.All))
                    Restart();

                break;

            case 2:
                ContinueImage.sprite = ContinueSprite;
                RestartImage.sprite = RestartSprite;
                ExitImage.sprite = ExitSprite_Gray;

                if (XCI.GetButtonDown(XboxButton.A, XboxController.All))
                    Exit();

                break;

        }

    }

    public void Continue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("PlayerMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
