using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Cross : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aModuleLinkDirection.Clear();
		aModuleLinkDirection.Add(ModuleDirection.UP);
		aModuleLinkDirection.Add(ModuleDirection.DOWN);
		aModuleLinkDirection.Add(ModuleDirection.LEFT);
		aModuleLinkDirection.Add(ModuleDirection.RIGHT);
	}

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
