using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_T_Type : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aModuleLinkDirection.Clear();
		switch (aModuleDirection)
		{
			case ModuleDirection.NO_DOWN:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				break;
			case ModuleDirection.NO_LEFT:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  
				break;
			case ModuleDirection.NO_UP:
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); 
				break;
			case ModuleDirection.NO_RIGHT:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); 
				break;
		}
	}

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
