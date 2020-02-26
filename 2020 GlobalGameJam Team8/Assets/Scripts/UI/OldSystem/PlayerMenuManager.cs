using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;


public class PlayerMenuManager : MonoBehaviour
{

    public Image p1Image;
    public Image p2Image;
    public Image p3Image;
    public Image p4Image;

    public Sprite p1Sprite;
    public Sprite p2Sprite;
    public Sprite p3Sprite;
    public Sprite p4Sprite;

    public Sprite p1Sprite_Gray;
    public Sprite p2Sprite_Gray;
    public Sprite p3Sprite_Gray;
    public Sprite p4Sprite_Gray;

    private int LevalID = 1;


    void Start()
    {
        
    }

    void Update()
    {

        CheckPlayerReady(ControllerType.Xbox_First);
        CheckPlayerReady(ControllerType.Xbox_Second);
        CheckPlayerReady(ControllerType.Xbox_Third);
        CheckPlayerReady(ControllerType.Xbox_Fourth);
        CheckPlayerReady(ControllerType.Keyboard1);
        CheckPlayerReady(ControllerType.Keyboard2);

        SetImage();

        CheckGameStart();


    }

    private void SetImage()
    {
        p1Image.sprite = p1Sprite_Gray;
        p2Image.sprite = p2Sprite_Gray;
        p3Image.sprite = p3Sprite_Gray;
        p4Image.sprite = p4Sprite_Gray;


        foreach (PlayerInfo val in PlayerManager.Instance.players)
        {
            switch (val.color)
            {
                case ColorType.Red:
                    p1Image.sprite = p1Sprite;
                    break;
                case ColorType.Yellow:
                    p2Image.sprite = p2Sprite;
                    break;
                case ColorType.Green:
                    p3Image.sprite = p3Sprite;
                    break;
                case ColorType.Blue:
                    p4Image.sprite = p4Sprite;
                    break;
            }
        }
    }

    private void CheckPlayerReady(ControllerType _controllerType)
    {
        bool inButton=false, outButton=false;
        switch (_controllerType)
        {
            case ControllerType.Xbox_First:
                inButton = XCI.GetButtonDown(XboxButton.A, XboxController.First);
                outButton = XCI.GetButtonDown(XboxButton.B, XboxController.First);
                break;
            case ControllerType.Xbox_Second:
                inButton = XCI.GetButtonDown(XboxButton.A, XboxController.Second);
                outButton = XCI.GetButtonDown(XboxButton.B, XboxController.Second);
                break;
            case ControllerType.Xbox_Third:
                inButton = XCI.GetButtonDown(XboxButton.A, XboxController.Third);
                outButton = XCI.GetButtonDown(XboxButton.B, XboxController.Third);
                break;
            case ControllerType.Xbox_Fourth:
                inButton = XCI.GetButtonDown(XboxButton.A, XboxController.Fourth);
                outButton = XCI.GetButtonDown(XboxButton.B, XboxController.Fourth);
                break;
            case ControllerType.Keyboard1:
                inButton = Input.GetKeyDown(KeyCode.G);
                outButton = Input.GetKeyDown(KeyCode.H);
                break;
            case ControllerType.Keyboard2:
                inButton = Input.GetKeyDown(KeyCode.Keypad1);
                outButton = Input.GetKeyDown(KeyCode.Keypad2);
                break;
        }

        if (inButton && PlayerManager.Instance.SearchPlayer(_controllerType) == null)
            PlayerManager.Instance.AddPlayer(_controllerType);
        else if (outButton && PlayerManager.Instance.SearchPlayer(_controllerType) != null)
            PlayerManager.Instance.RemovePlayer(PlayerManager.Instance.SearchPlayer(_controllerType));     

    }


    /// <summary>
    /// 檢查遊戲是否開始
    /// </summary>
    public void CheckGameStart()
    {
        if (PlayerManager.Instance.players.Count > 0)
        {
            if (XCI.GetButton(XboxButton.Start, XboxController.First))
            {
                GoToGameScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LevalID = 1;
                GoToGameScene();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LevalID = 2;
                GoToGameScene();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LevalID = 3;
                GoToGameScene();
            }
        }
    }

    /// <summary>
    /// 進入遊戲場景
    /// </summary>
    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene"+LevalID);
    }

}
