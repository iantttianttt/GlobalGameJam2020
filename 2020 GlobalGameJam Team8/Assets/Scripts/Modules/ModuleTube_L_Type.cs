using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_L_Type : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mModuleLinkDirection.Clear();
		switch (ModuleDirection)
		{
			case EModuleDirection.UP_TO_RIGHT:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT); 
			break;
			case EModuleDirection.RIGHT_TO_UP:
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				break;
			case EModuleDirection.RIGHT_TO_DOWN: 
				this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f)); 
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				break;
			case EModuleDirection.DOWN_TO_RIGHT: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f)); 
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.RIGHT);
				break;
			case EModuleDirection.DOWN_TO_LEFT:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f));
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				break;
			case EModuleDirection.LEFT_TO_DOWN:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f));
				mModuleLinkDirection.Add(EModuleDirection.DOWN);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				break;
			case EModuleDirection.LEFT_TO_UP:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f));
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				break;
			case EModuleDirection.UP_TO_LEFT:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f));
				mModuleLinkDirection.Add(EModuleDirection.UP);
				mModuleLinkDirection.Add(EModuleDirection.LEFT);
				break;
		}
	}

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (mModuleDirection)
		{
			case EModuleDirection.UP_TO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				break;
			case EModuleDirection.LEFT_TO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				break;

			case EModuleDirection.LEFT_TO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;
			case EModuleDirection.DOWN_TO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(mModuleIndex));
				break;

			case EModuleDirection.DOWN_TO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
			case EModuleDirection.RIGHT_TO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(mModuleIndex));
				break;

			case EModuleDirection.RIGHT_TO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				break;
			case EModuleDirection.UP_TO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(mModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(mModuleIndex));
				break;
		}
	}
}
