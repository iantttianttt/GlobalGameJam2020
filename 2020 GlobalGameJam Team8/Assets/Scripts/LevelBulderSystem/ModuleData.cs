using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成模組時匯入的資料
/// </summary>
[System.Serializable]
public class ModuleData
{
    public EModuleType      ModuleType = EModuleType.NONE;
    public Vector2         SetUpIndex = new Vector2(0f, 0f);
    public EModuleDirection ModuleDirection;

    [Space(20)] // For Conveyor and Creater
    public Vector2         ModuleCreateTimer = new Vector2(3.0f, 4.5f);
    public ModuleRaito[]   ModuleCreateList;
    public float           ConveyorSpeed     = 10.0f;

    [Space(20)] // For Boss
    public Vector2 BossTimer = new Vector2(60, 90);
    public float   BossMessBost = 2.0f;
}


[System.Serializable]
public class ModuleRaito
{
    public float ModuleCreateRaito;
    public EModuleType ModuleType;
}