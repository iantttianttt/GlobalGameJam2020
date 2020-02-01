using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;


public class PlayerMenuManager : MonoBehaviour
{
    private PlayerInfo P1;
    private PlayerInfo P2;
    private PlayerInfo P3;
    private PlayerInfo P4;

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

    void Start()
    {
        P1 = new PlayerInfo(XboxController.First);
        P2 = new PlayerInfo(XboxController.Second);
        P3 = new PlayerInfo(XboxController.Third);
        P4 = new PlayerInfo(XboxController.Fourth);
    }

    void Update()
    {
        ///個別檢查控制器是否載入
        CheckControllerPlugged(P1, p1Image);
        CheckControllerPlugged(P2, p2Image);
        CheckControllerPlugged(P3, p3Image);
        CheckControllerPlugged(P4, p4Image);

        if (PlayerManager.Instance.players.Count > 1)
        {
            CheckGameStart();
        }
    }

    /// <summary>
    /// 檢查控制器是否載入
    /// </summary>
    /// <param name="player">檢查對象</param>
    /// <param name="image">顯示圖片</param>
    private void CheckControllerPlugged(PlayerInfo player,Image image)
    {
        if(XCI.IsPluggedIn(player.controller))
        {
            if(XCI.GetButtonDown(XboxButton.A, player.controller))
            {
                ///判定玩家是否加入遊戲
                if (player.inGame)
                {
                    player.inGame = false;
                    PlayerManager.Instance.RemovePlayer(player);
                }
                else
                {
                    player.inGame = true;
                    PlayerManager.Instance.AddPlayer(player);
                }
            }
        }
        else
        {
            player.inGame = false;
            PlayerManager.Instance.RemovePlayer(player);
        }

        ///調整選單玩家顏色
        if (player.inGame)
        {
            switch (player.controller) {

                case XboxController.First:
                    image.sprite = p1Sprite;
                    break;
                case XboxController.Second:
                    image.sprite = p2Sprite;
                    break;
                case XboxController.Third:
                    image.sprite = p3Sprite;
                    break;
                case XboxController.Fourth:
                    image.sprite = p4Sprite;
                    break;
            }
        }
        else
        {
            switch (player.controller)
            {
                case XboxController.First:
                    image.sprite = p1Sprite_Gray;
                    break;
                case XboxController.Second:
                    image.sprite = p2Sprite_Gray;
                    break;
                case XboxController.Third:
                    image.sprite = p3Sprite_Gray;
                    break;
                case XboxController.Fourth:
                    image.sprite = p4Sprite_Gray;
                    break;
            }
        }
    }
       
    /// <summary>
    /// 檢查遊戲是否開始
    /// </summary>
    public void CheckGameStart()
    {
        foreach(PlayerInfo val in PlayerManager.Instance.players)
        {
            if (XCI.GetButton(XboxButton.Start, val.controller))
            {
                GoToGameScene();
            }
        }
       
    }

    /// <summary>
    /// 進入遊戲場景
    /// </summary>
    public void GoToGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
