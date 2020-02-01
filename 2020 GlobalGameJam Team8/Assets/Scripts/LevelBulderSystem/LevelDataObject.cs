using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="GlobalGameJam2020/Create LevelDataObject")]
public class LevelDataObject : ScriptableObject
{
    public LevelData LevelData;
}


[System.Serializable]
public class LevelData
{
    public string  LevelName;
    public Vector2 LevelLayout     = new Vector2(13,9);

    [Space(20)]
    public float   SteamStartTimer = 20.0f;
    public float   SteamBreakTimer = 10.0f;

    [Space(20)]
    public ModuleData[] DefaultModule;
}