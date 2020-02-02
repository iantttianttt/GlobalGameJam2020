using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_End : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aAutoUpdate = false;
	}


	public override void UpdateModule()
	{
		base.UpdateModule();
		Debug.Log("OMG!!!!!!!!!!!!!!!!!  YOOUUUU WWWWIINNNN!!!!!!");
	}

}
