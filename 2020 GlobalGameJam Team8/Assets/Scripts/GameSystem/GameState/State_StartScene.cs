using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class State_StartScene : IGameState
{
	public State_StartScene(GameManager Controller):base(Controller)
	{
		this.StateName = "StartScene";
	}

	// 準備(讀取場景等)
	public override IEnumerator StatePrepare()
	{
		AsyncOperation asyncLoad = SceneController.Instance.LoadScene(SceneController.SCENE_NAME_START_MENU);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		UIManager.Instance.InitUIManager();
		mStatePrepared = true;
	}

	// 開始
	public override void StateEnter()
	{
		UIManager.Instance.ShowPanel(EUIPanelType.START_MENU);
	}

	// 結束
	public override void StateExit()
	{
		UIManager.Instance.ClearAllPanel();
	}

	// 更新
	public override void StateUpdate()
	{	
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Second)||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Fourth)||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Third) ||
            Input.GetKeyDown(KeyCode.G)                               || 
			Input.GetKeyDown(KeyCode.Keypad1))
		{
            GameManager.Instance.ChangeGameState(new State_PlayerSelect(GameManager.Instance));
		}
	}
}
