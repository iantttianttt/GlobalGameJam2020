using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_End : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aModuleLinkDirection.Clear();
		switch (aModuleDirection)
		{
			case ModuleDirection.UP:
				aModuleLinkDirection.Add(ModuleDirection.UP);
				break;
			case ModuleDirection.DOWN:
				aModuleLinkDirection.Add(ModuleDirection.DOWN);
				break;
			case ModuleDirection.RIGHT:
				aModuleLinkDirection.Add(ModuleDirection.RIGHT);
				break;
			case ModuleDirection.LEFT:
				aModuleLinkDirection.Add(ModuleDirection.LEFT);
				break;
		}
				
		aAutoUpdate = false;
	}


	public override void UpdateModule()
	{
		base.UpdateModule();
		Debug.Log("OMG!!!!!!!!!!!!!!!!!  YOOUUUU WWWWIINNNN!!!!!!");
	}

}
