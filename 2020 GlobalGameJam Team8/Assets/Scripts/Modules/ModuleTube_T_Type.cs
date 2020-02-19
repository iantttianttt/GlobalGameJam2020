using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_T_Type : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mModuleLinkDirection.Clear();
		switch (mModuleDirection)
		{
			case EModuleDirection.NO_DOWN:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				break;
			case EModuleDirection.NO_LEFT:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  
				break;
			case EModuleDirection.NO_UP:
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); 
				break;
			case EModuleDirection.NO_RIGHT:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); 
				break;
		}
	}

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (mModuleDirection)
		{
			case EModuleDirection.NO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
			case EModuleDirection.NO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				break;
			case EModuleDirection.NO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
			case EModuleDirection.NO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
		}
	}

}
