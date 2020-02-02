using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Start: ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aAutoUpdate = false;
		GameManager.Instance.ModuleTube_Start = this;
	}

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (aModuleDirection)
		{
			case ModuleDirection.UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				break;
			case ModuleDirection.DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				break;
			case ModuleDirection.LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex)); 
				break;
			case ModuleDirection.RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
		}
	}

}
