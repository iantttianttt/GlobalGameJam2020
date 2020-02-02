using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleConveyor : ModuleBase
{
	public bool IsLine;

	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		if (IsLine)
		{
			switch (aModuleDirection)
			{
				case ModuleDirection.UP_TO_DOWN: break;
				case ModuleDirection.DOWN_TO_UP:    this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
				case ModuleDirection.LEFT_TO_RIGHT: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
				case ModuleDirection.RIGHT_TO_LEFT: this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
			}
		}
		else
		{
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
	}

	public override void SetUpModule()
	{
		base.SetUpModule();
	}

	public override void UpdateModule()
	{
		base.UpdateModule();
	}

}
