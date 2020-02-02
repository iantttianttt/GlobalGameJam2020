using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_Line : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aModuleLinkDirection.Clear();
		switch (aModuleDirection)
		{
			case ModuleDirection.UP_TO_DOWN:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				break;
			case ModuleDirection.DOWN_TO_UP:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); 
				break;
			case ModuleDirection.LEFT_TO_RIGHT:
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  
				break;
			case ModuleDirection.RIGHT_TO_LEFT:
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); 
				break;
		}
	}

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
