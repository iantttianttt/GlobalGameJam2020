using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_GamePlay : IGameState
{
	public State_GamePlay(GameManager Controller):base(Controller)
	{
		this.StateName = "GamePlay";
	}
	// 準備(讀取場景等)
	public override IEnumerator StatePrepare()
	{
        AsyncOperation asyncLoad = SceneController.Instance.LoadScene(SceneController.SCENE_NAME_GAME_SCENE, true);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
		GameController.Instance.InitNewGame("001");
        yield return new WaitForSeconds(LEVEL_BUILD_TIME);
		mStatePrepared = true;
	}

	// 開始
	public override void StateEnter()
	{
		GameController.Instance.GameStart();
	}

	// 結束
	public override void StateExit()
	{
		GameController.Instance.ClearGamePlayLevel();

	}
			
	// 更新
	public override void StateUpdate()
	{	
        GameController.Instance.UpdateController();
	}

	//-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float  LEVEL_BUILD_TIME = 1.0f;

}
