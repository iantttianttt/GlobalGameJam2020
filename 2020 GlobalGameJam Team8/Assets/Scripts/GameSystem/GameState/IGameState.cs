using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IGameState
{
	//-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------
	public string StateName     { get { return mStateName;     } set{ mStateName = value; } }
	public bool   StatePrepared { get { return mStatePrepared; }                            }

	// 建構者
	public IGameState(GameManager _GameManager)
	{ 
		mGameManager = _GameManager; 
	}

	// 準備(讀取場景等)
	public virtual IEnumerator StatePrepare()
	{
		yield return new WaitForSeconds(0.0f);
		mStatePrepared = true;
	}

	// 開始
	public virtual void StateEnter()
	{}

	// 結束
	public virtual void StateExit()
	{}

	// 更新
	public virtual void StateUpdate()
	{}

	public override string ToString ()
	{
		return string.Format ("[IGameState: StateName={0}]", StateName);
	}

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
	private string        mStateName     = "IGameState";
	protected GameManager mGameManager   = null;
	protected bool        mStatePrepared = false;


}
