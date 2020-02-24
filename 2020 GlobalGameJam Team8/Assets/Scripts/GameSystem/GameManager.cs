using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{

    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化遊戲
    /// </summary>
	public void InitGameManager()
	{
        ChangeGameState(new State_StartScene(this));
	}

    /// <summary>
    /// 切換遊戲狀態
    /// </summary>
	public void ChangeGameState(IGameState _NewState)
	{
		mhasRunEnter = false;

		// 通知前一個State結束
		if( mGameState != null )
        {
			mGameState.StateExit();
            StartCoroutine(_NewState.StatePrepare());
        }
        else
        {
            StartCoroutine(_NewState.StatePrepare());     
        }
		// 設定
		mGameState = _NewState;	
	}

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    private void Update()
    {
        GameLoop();
    }

    /// <summary>
    /// 遊戲主迴圈
    /// </summary>
	private void GameLoop()
	{
		if(SceneController.Instance.SceneLoadingHold)
        {
			return ;
        }

		// 通知新的State開始
		if( mGameState != null && !mhasRunEnter && mGameState.StatePrepared)
		{
			mGameState.StateEnter();
			mhasRunEnter = true;
		}

		// 更新
		if( mGameState != null && mhasRunEnter)
        {
			mGameState.StateUpdate();
        }
	}


    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private IGameState mGameState;	
	private bool 	   mhasRunEnter = false;


    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------

}
