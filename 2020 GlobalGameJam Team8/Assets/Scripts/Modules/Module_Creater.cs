using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module_Creater : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);
		aAutoUpdate = true;
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
		if (targetData.ModuleType != ModuleType.NONE)
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
				case ModuleDirection.UP:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x, this.ModuleIndex.y + 1), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case ModuleDirection.DOWN:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x, this.ModuleIndex.y - 1), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case ModuleDirection.LEFT:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x - 1, this.ModuleIndex.y), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
				case ModuleDirection.RIGHT:
					if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(this.ModuleIndex.x + 1, this.ModuleIndex.y), out conveyorTarget))
					{
						newModule.AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
					}
					break;
			}
			newModule.ConveyorSpeed = this.aConveyorSpeed;
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
			case ModuleType.TUBE_LINE:
				num = Random.Range(0, 2);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = ModuleDirection.UP_TO_DOWN;    break;
					case 1: targetModuleData.ModuleDirection = ModuleDirection.LEFT_TO_RIGHT; break;
				}
				break;
			case ModuleType.TUBE_L_TYPE:
				num = Random.Range(0, 4);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = ModuleDirection.UP_TO_RIGHT;   break;
					case 1: targetModuleData.ModuleDirection = ModuleDirection.RIGHT_TO_DOWN; break;
					case 2: targetModuleData.ModuleDirection = ModuleDirection.DOWN_TO_LEFT;  break;
					case 3: targetModuleData.ModuleDirection = ModuleDirection.LEFT_TO_UP;    break;
				}
				break;
			case ModuleType.TUBE_T_TYPE:
				num = Random.Range(0, 4);
				switch (num)
				{
					case 0: targetModuleData.ModuleDirection = ModuleDirection.NO_UP;    break;
					case 1: targetModuleData.ModuleDirection = ModuleDirection.NO_RIGHT; break;
					case 2: targetModuleData.ModuleDirection = ModuleDirection.NO_DOWN;  break;
					case 3: targetModuleData.ModuleDirection = ModuleDirection.NO_LEFT;  break;
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
