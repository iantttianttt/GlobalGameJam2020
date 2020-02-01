using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Cross : ModuleBase
{

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (aModuleDirection)
		{
			case ModuleDirection.CROSS:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
		}
	}

}
