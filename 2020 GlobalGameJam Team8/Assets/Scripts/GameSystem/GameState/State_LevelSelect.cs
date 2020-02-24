using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class State_LevelSelect : IGameState
{
	public State_LevelSelect(GameManager Controller):base(Controller)
	{
		this.StateName = "LevelSelect";
	}

	// 開始
	public override void StateEnter()
	{

	}

	// 結束
	public override void StateExit()
	{

	}
			
	// 更新
	public override void StateUpdate()
	{	
	}


    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------

}
