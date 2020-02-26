using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_PlayerSelect : IUIPanel
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------
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

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Consturcter
    //-----------------------------------------------------------------------
    public UIPanel_PlayerSelect()
	{
		mUIPanelType = EUIPanelType.PLAYER_SELECT_MENU;
	}

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化遊戲
    /// </summary>
    public void SetImage()
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

}
