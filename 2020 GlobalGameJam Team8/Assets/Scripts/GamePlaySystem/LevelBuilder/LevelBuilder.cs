using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : Singleton<LevelBuilder>
{
    // TODO 關卡背景生成修正



    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化 Level Builder
    /// </summary>
    public void InitLevelBuilder()
    {
        Floor = Resources.Load<GameObject>(FLOOR_OBJECT_PATH);
        Wall  = Resources.Load<GameObject>(WALL_OBJECT_PATH);
    }

    /// <summary>
    /// 設定場景
    /// </summary>
    public void BuildLevel(LevelData iLevelData)
    {
        // Spawn Floor
        foreach(Vector3 values in ModuleManager.Instance.ModulePositionData.Values)
        {
            Instantiate(Floor, new Vector3(values.x, FLOOR_SPAWN_HIGH, values.z), Quaternion.identity);
        }

        // Spawn Wall
        Vector3 upRight;
        Vector3 downLeft;
        ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(iLevelData.LevelLayout.x, iLevelData.LevelLayout.y), out upRight);
        ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(1, 1), out downLeft);

        for(int i = 0; i < iLevelData.LevelLayout.x + 2 ; i++)
        {
            Vector3 spawnPosDown = new Vector3( (downLeft.x - MODULE_SIZE) + (i * MODULE_SIZE), WALL_SPAWN_HIGH, downLeft.z - MODULE_SIZE);
            Instantiate(Wall, spawnPosDown, Quaternion.identity);
            Vector3 spawnPosUp = new Vector3( (downLeft.x - MODULE_SIZE) + (i * MODULE_SIZE), WALL_SPAWN_HIGH, upRight.z + MODULE_SIZE);
            Instantiate(Wall, spawnPosUp, Quaternion.identity);
        }
        for(int i = 0; i < iLevelData.LevelLayout.y ; i++)
        {
            Vector3 spawnPosLeft = new Vector3( downLeft.x - MODULE_SIZE, WALL_SPAWN_HIGH, (downLeft.z + i * MODULE_SIZE));
            Instantiate(Wall, spawnPosLeft, Quaternion.identity);
            Vector3 spawnPosRight = new Vector3( upRight.x + MODULE_SIZE, WALL_SPAWN_HIGH, (downLeft.z + i * MODULE_SIZE));
            Instantiate(Wall, spawnPosRight, Quaternion.identity);
        }
    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private GameObject Floor;
    private GameObject Wall;

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const string FLOOR_OBJECT_PATH = "Prefabs/Floor";
    private const string WALL_OBJECT_PATH  = "Prefabs/Wall";
    private const float  MODULE_SIZE       = 1.0f;
    private const float  FLOOR_SPAWN_HIGH  = -0.5f;
    private const float  WALL_SPAWN_HIGH   = -1.0f;
}
