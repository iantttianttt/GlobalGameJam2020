using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Line : ModuleBase
{

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (aModuleDirection)
		{
			case ModuleDirection.DOWN_TO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				break;
			case ModuleDirection.UP_TO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				break;
		}
	}

}
