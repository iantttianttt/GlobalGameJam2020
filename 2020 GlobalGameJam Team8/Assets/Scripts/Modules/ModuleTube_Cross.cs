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
		List<ModuleBase> nextModule = new List<ModuleBase>();

		//取得周邊模組清單
		nextModule.Add(ModuleManager.Instance.GetUpModule(mModuleIndex));
		nextModule.Add(ModuleManager.Instance.GetDownModule(mModuleIndex));
		nextModule.Add(ModuleManager.Instance.GetRightModule(mModuleIndex));
		nextModule.Add(ModuleManager.Instance.GetLeftModule(mModuleIndex));	

		//更新周邊模組
		foreach(ModuleBase hasValue in nextModule)
		{
			if(hasValue != null)
			{
				ModuleManager.Instance.UpdatingModule.Add(hasValue);
				bool updateSuss = ModuleManager.Instance.RequestModuleUpdate(hasValue);
				if(updateSuss)
				{
					ModuleManager.Instance.AddLinkCount();
				}
			}
		}
		
		//確認更新是否結束
		ModuleManager.Instance.CheckIsLinkCheckOver();
	}

}
