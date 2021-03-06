﻿using System.Collections;
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
			switch (mModuleDirection)
			{
				case EModuleDirection.UP_TO_DOWN: break;
				case EModuleDirection.DOWN_TO_UP:    this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
				case EModuleDirection.LEFT_TO_RIGHT: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
				case EModuleDirection.RIGHT_TO_LEFT: this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
			}
		}
		else
		{
			switch (mModuleDirection)
			{
				case EModuleDirection.UP_TO_RIGHT: break;
				case EModuleDirection.RIGHT_TO_UP: break;
				case EModuleDirection.RIGHT_TO_DOWN: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
				case EModuleDirection.DOWN_TO_RIGHT: this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));  break;
				case EModuleDirection.DOWN_TO_LEFT:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
				case EModuleDirection.LEFT_TO_DOWN:  this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f)); break;
				case EModuleDirection.LEFT_TO_UP:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
				case EModuleDirection.UP_TO_LEFT:    this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f)); break;
			}
		}
	}


	public override void UpdateModule()
	{
		base.UpdateModule();
	}

}
