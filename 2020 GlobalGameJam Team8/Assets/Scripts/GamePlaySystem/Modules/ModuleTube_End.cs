using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_End : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mModuleLinkDirection.Clear();

		switch (mModuleDirection)
		{
			case EModuleDirection.UP:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				break;
			case EModuleDirection.DOWN:
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				break;
			case EModuleDirection.RIGHT:
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				break;
			case EModuleDirection.LEFT:
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				break;
		}
				
		mAutoUpdate = false;
	}


	public override void UpdateModule()
	{
		base.UpdateModule();
		Debug.Log("Win");
		GameManager.Instance.ChangeGameState(new State_PlayerSelect(GameManager.Instance));
//		GameController.Instance.mainUI.Win();
	}

}
