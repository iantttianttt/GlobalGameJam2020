using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTube_L_Type : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		switch (aModuleDirection)
		{
			case ModuleDirection.UP_TO_RIGHT: break;
			case ModuleDirection.RIGHT_TO_UP: break;
			case ModuleDirection.RIGHT_TO_DOWN: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
			case ModuleDirection.DOWN_TO_RIGHT: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
			case ModuleDirection.DOWN_TO_LEFT:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
			case ModuleDirection.LEFT_TO_DOWN:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
			case ModuleDirection.LEFT_TO_UP:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
			case ModuleDirection.UP_TO_LEFT:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
		}
	}

	public override void UpdateModule()
	{
		base.UpdateModule();

		switch (aModuleDirection)
		{
			case ModuleDirection.UP_TO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				break;
			case ModuleDirection.LEFT_TO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				break;

			case ModuleDirection.LEFT_TO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				break;
			case ModuleDirection.DOWN_TO_LEFT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetLeftModule(aModuleIndex));
				break;

			case ModuleDirection.DOWN_TO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
			case ModuleDirection.RIGHT_TO_DOWN:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetDownModule(aModuleIndex));
				break;

			case ModuleDirection.RIGHT_TO_UP:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				break;
			case ModuleDirection.UP_TO_RIGHT:
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetUpModule(aModuleIndex));
				ModuleManager.Instance.RequestModuleUpdate(ModuleManager.Instance.GetRightModule(aModuleIndex));
				break;
		}
	}
}
