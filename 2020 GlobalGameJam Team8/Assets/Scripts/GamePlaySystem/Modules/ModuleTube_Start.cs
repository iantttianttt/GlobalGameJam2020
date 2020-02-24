using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Start: ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mAutoUpdate = false;
	}

	public override void UpdateModule()
	{
		base.UpdateModule();
		List<ModuleBase> nextModule = new List<ModuleBase>();

		//取得周邊模組清單
		switch (mModuleDirection)
		{
			case EModuleDirection.UP:
				nextModule.Add(ModuleManager.Instance.GetUpModule(mModuleIndex));
				break;
			case EModuleDirection.DOWN:
				nextModule.Add(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;
			case EModuleDirection.LEFT:
				nextModule.Add(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				break;
			case EModuleDirection.RIGHT:
				nextModule.Add(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
		}

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
