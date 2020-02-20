using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module_Creater : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		mAutoUpdate       = true;
		ModuleCreateTimer = iModuleData.ModuleCreateTimer;
		ModuleCreateList  = iModuleData.ModuleCreateList;
		ConveyorSpeed     = iModuleData.ConveyorSpeed;
	}

	public override void UpdateModule()
	{
		base.UpdateModule();
		aCreateTimer += Time.deltaTime;
		if (aCreateTimer >= aCurrentTargetTime)
		{
			SpawnNewModule();
			aCreateTimer = 0.0f;
			aCurrentTargetTime = Random.Range(ModuleCreateTimer.x, ModuleCreateTimer.y);
		}
	}

	private void SpawnNewModule()
	{
		ModuleData targetData = GetSpawnModuleData();
		if (targetData.ModuleType != EModuleType.NONE)
		{
			ModuleBase newModule = ModuleManager.Instance.RequestSpawnModule(targetData,this);
			if (newModule == null)
			{
				Debug.LogError("Spawn New Module Error!!!");
				return;
			}

			newModule.IsOnConveyor = true;

			Vector3 conveyorTarget = new Vector3(0.0f,0.0f, 0.0f);
			switch (this.ModuleDirection)
			{
				case EModuleDirection.UP:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x, this.ModuleIndex.y + 1), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case EModuleDirection.DOWN:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x, this.ModuleIndex.y - 1), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case EModuleDirection.LEFT:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x - 1, this.ModuleIndex.y), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case EModuleDirection.RIGHT:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x + 1, this.ModuleIndex.y), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
			}
			newModule.ConveyorSpeed = this.mConveyorSpeed;
		}
	}

	private ModuleData GetSpawnModuleData()
	{
		ModuleData targetModuleData = new ModuleData();

		float totalRaito = 0.0f;
		foreach (ModuleRaito item in ModuleCreateList)
		{
			totalRaito += item.ModuleCreateRaito;
		}
		float raito = totalRaito / 100.0f;


		float targetRaito = Random.Range(0.0f, 100.0f);
		float calculateRatio = 0.0f;
		foreach (ModuleRaito item in ModuleCreateList)
		{
			calculateRatio += item.ModuleCreateRaito / raito;
			if (calculateRatio >= targetRaito)
			{
				targetModuleData.ModuleType = item.ModuleType;
				break;
			}
		}

		int num = 0;
		switch (targetModuleData.ModuleType)
		{
			case EModuleType.TUBE_LINE:
				num = Random.Range(0, 2);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = EModuleDirection.UP_TO_DOWN;    break;
					case 1: targetModuleData.ModuleDirection = EModuleDirection.LEFT_TO_RIGHT; break;
				}
				break;
			case EModuleType.TUBE_L_TYPE:
				num = Random.Range(0, 4);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = EModuleDirection.UP_TO_RIGHT;   break;
					case 1: targetModuleData.ModuleDirection = EModuleDirection.RIGHT_TO_DOWN; break;
					case 2: targetModuleData.ModuleDirection = EModuleDirection.DOWN_TO_LEFT;  break;
					case 3: targetModuleData.ModuleDirection = EModuleDirection.LEFT_TO_UP;    break;
				}
				break;
			case EModuleType.TUBE_T_TYPE:
				num = Random.Range(0, 4);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = EModuleDirection.NO_UP;    break;
					case 1: targetModuleData.ModuleDirection = EModuleDirection.NO_RIGHT; break;
					case 2: targetModuleData.ModuleDirection = EModuleDirection.NO_DOWN;  break;
					case 3: targetModuleData.ModuleDirection = EModuleDirection.NO_LEFT;  break;
				}
				break;
		}
		return targetModuleData;
	}

	private Vector2       ModuleCreateTimer;
	private ModuleRaito[] ModuleCreateList;

	private float aCurrentTargetTime;
	private float aCreateTimer;

	//-----------------------------------------------------------------------
	// Const
	//-----------------------------------------------------------------------
	private const float MODULE_ON_CONVEYOR_HIGH = 0.5f;
}
