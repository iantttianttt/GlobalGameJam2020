using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Line : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mModuleLinkDirection.Clear();
		switch (ModuleDirection)
		{
			case EModuleDirection.UP_TO_DOWN:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				break;
			case EModuleDirection.DOWN_TO_UP:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); 
				break;
			case EModuleDirection.LEFT_TO_RIGHT:
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  
				break;
			case EModuleDirection.RIGHT_TO_LEFT:
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); 
				break;
		}
	}

	public override void UpdateModule()
	{
		base.UpdateModule();
		List<ModuleBase> nextModule = new List<ModuleBase>();
		//取得周邊模組清單
		switch (mModuleDirection)
		{
			case EModuleDirection.DOWN_TO_UP:
				nextModule.Add(ModuleManager.Instance.GetUpModule(mModuleIndex));
				nextModule.Add(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;
			case EModuleDirection.UP_TO_DOWN:
				nextModule.Add(ModuleManager.Instance.GetUpModule(mModuleIndex));
				nextModule.Add(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;
			case EModuleDirection.LEFT_TO_RIGHT:
				nextModule.Add(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				nextModule.Add(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
			case EModuleDirection.RIGHT_TO_LEFT:
				nextModule.Add(ModuleManager.Instance.GetRightModule(mModuleIndex));
				nextModule.Add(ModuleManager.Instance.GetLeftModule(mModuleIndex));
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
