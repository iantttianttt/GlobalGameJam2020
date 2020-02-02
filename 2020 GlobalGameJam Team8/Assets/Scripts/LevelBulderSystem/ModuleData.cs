using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    public ModuleType      ModuleType = ModuleType.NONE;
    public Vector2         SetUpIndex = new Vector2(0f, 0f);
    public ModuleDirection ModuleDirection;

    [Space(20)] // For Conveyor and Creater
    public Vector2       ModuleCreateTimer = new Vector2(3.0f, 4.5f);
    public ModuleRaito[] ModuleCreateList;
    public float         ConveyorSpeed     = 10.0f;

    [Space(20)] // For Boss
    public Vector2 BossTimer = new Vector2(60, 90);
    public float   BossMessBost = 2.0f;
}


[System.Serializable]
public class ModuleRaito
{
    public float ModuleCreateRaito;
    public ModuleType ModuleType;
}