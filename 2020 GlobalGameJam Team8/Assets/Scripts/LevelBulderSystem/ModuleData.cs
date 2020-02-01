using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    public ModuleType ModuleType;
    public Vector2 SetUpIndex;
    public ModuleDirection ModuleDirection;

    [Space(20)]
    public Vector2 ModleCreateTimer = new Vector2(3.0f, 4.5f);
    public ModuleRaito[] ModleCreateList;
    public float ConveyorSpeed = 10.0f;

    [Space(20)]
    public Vector2 BossTimer = new Vector2(60, 90);
    public float BossMessBost = 2.0f;
}


[System.Serializable]
public class ModuleRaito
{
    public float ModuleCreateRaito;
    public ModuleType ModuleType;
}