using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Start: ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mAutoUpdate = false;
		GameManager.Instance.ModuleTube_Start = this;
	}

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (mModuleDirection)
		{
			case EModuleDirection.UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				break;
			case EModuleDirection.DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;
			case EModuleDirection.LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex)); 
				break;
			case EModuleDirection.RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
		}
	}

}
