using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Cross : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mModuleLinkDirection.Clear();
		mModuleLinkDirection.Add(EModuleDirection.UP);
		mModuleLinkDirection.Add(EModuleDirection.DOWN);
		mModuleLinkDirection.Add(EModuleDirection.LEFT);
		mModuleLinkDirection.Add(EModuleDirection.RIGHT);
	}

	public override void UpdateModule()
	{
		base.UpdateModule();
		switch (mModuleDirection)
		{
			case EModuleDirection.UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
		}
	}

}
