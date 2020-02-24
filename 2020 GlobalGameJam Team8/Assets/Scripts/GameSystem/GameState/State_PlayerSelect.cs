using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class State_PlayerSelect : IGameState
{
	public State_PlayerSelect(GameManager Controller):base(Controller)
	{
		this.StateName = "PlayerSelect";
	}

	// 準備(讀取場景等)
	public override IEnumerator StatePrepare()
	{
        AsyncOperation asyncLoad = SceneController.Instance.LoadScene(SceneController.SCENE_NAME_PLAYER_SELECT);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
		mStatePrepared = true;
	}

	// 開始
	public override void StateEnter()
	{
		SceneController.Instance.LoadScene(SceneController.SCENE_NAME_PLAYER_SELECT);
		base.StateEnter();
	}

	// 結束
	public override void StateExit()
	{

	}
			
	// 更新
	public override void StateUpdate()
	{	
        CheckPlayerReady(ControllerType.Xbox_First);
        CheckPlayerReady(ControllerType.Xbox_Second);
        CheckPlayerReady(ControllerType.Xbox_Third);
        CheckPlayerReady(ControllerType.Xbox_Fourth);
        CheckPlayerReady(ControllerType.Keyboard1);
        CheckPlayerReady(ControllerType.Keyboard2);

        CheckPlayerSelectFinish();
	}


    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
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
		{
            PlayerManager.Instance.AddPlayer(_controllerType);
		}
		else if (outButton && PlayerManager.Instance.SearchPlayer(_controllerType) != null)
        {
		    PlayerManager.Instance.RemovePlayer(PlayerManager.Instance.SearchPlayer(_controllerType));     
		}
    }

    /// <summary>
    /// 檢查角色選擇是否完成
    /// </summary>
    private void CheckPlayerSelectFinish()
    {
        if (PlayerManager.Instance.players.Count > 0)
        {
            if (XCI.GetButton(XboxButton.Start, XboxController.First) || Input.GetKeyDown(KeyCode.Space))
            {
                //GameManager.Instance.ChangeGameState(new State_LevelSelect(GameManager.Instance));
                GameManager.Instance.ChangeGameState(new State_GamePlay(GameManager.Instance));
            }
        }
    }


}
