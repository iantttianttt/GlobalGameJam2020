using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_T_Type : ModuleBase
{

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (aModuleDirection)
		{
			case ModuleDirection.NO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
			case ModuleDirection.NO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				break;
			case ModuleDirection.NO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
			case ModuleDirection.NO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
		}
	}

}
