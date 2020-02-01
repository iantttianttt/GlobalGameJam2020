using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModuleManager : Singleton<ModuleManager>
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------


    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------
    public bool      GetIsSetUpFinish    { get { return aIsSetUpFinish;    }}
    public LevelData GetCurrentLevelData { get { return aCurrentLevelData; }} //TODO
    public GameObject Floor;
    public LevelData AA;

    //-----------------------------------------------------------------------
    //Public Function
    //-----------------------------------------------------------------------
    public void InitModuleManager()
    {
        aIsSetUpFinish = false;
    }

    public void BuildLevel(LevelData iLevelData)
    {
        aCurrentLevelData = iLevelData;
        CalculateModulePositionData(aCurrentLevelData);
        aIsSetUpFinish = true;
    }

    public void ClearLevel()
    {
        
    }

    public void ResetLevel()
    {
        
    }


    public ModuleBase GetUpModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish) { return null; }
        return null;
    }

    public ModuleBase GetDownModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish) { return null; }
        return null;
    }

    public ModuleBase GetLeftModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish) { return null; }
        return null;
    }
    public ModuleBase GetRightModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish) { return null; }
        return null;
    }

    public bool RequestModuleUpdate(ModuleBase iTargetModule)
    {
        if (!aIsSetUpFinish) { return false; }
        if (aUpdatedModule.ContainsKey(iTargetModule))
        {
            bool hasUpdated = false;
            if (aUpdatedModule.TryGetValue(iTargetModule, out hasUpdated))
            {
                if (!hasUpdated)
                {
                    iTargetModule.UpdateModule();
                    aUpdatedModule.Remove(iTargetModule);
                    aUpdatedModule.Add(iTargetModule, true);
                    return true;
                }
            }
        }
        return false;
    }

    public void ModuleAutoUpdate()
    {
        if (!aIsSetUpFinish) { return; }
        foreach (var OneItem in aModuleList)
        {
            ModuleBase hasValue = null;
            aModuleList.TryGetValue(OneItem.Key, out hasValue);
            if (hasValue && hasValue.AutoUpdateModule)
            {
                RequestModuleUpdate(hasValue);
            }
        }
    }

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    private void CalculateModulePositionData(LevelData iLevelData)
    {
        int horizontalCount = (int)iLevelData.LevelLayout.x;
        int verticalCount   = (int)iLevelData.LevelLayout.y;

        for (int i = 0; i < horizontalCount * verticalCount; i++)
        {
            float xPosition = 0.0f;
            float zPosition = 0.0f;

            if (horizontalCount % 2 == 0)  // X是偶數
            {
                if ((i % horizontalCount) < (horizontalCount / 2))
                {   // 落點左邊
                    xPosition = ((horizontalCount / 2) - (i % horizontalCount)) * -MODULE_SIZE + MODULE_SIZE * 0.5f;
                }
                else
                {   // 落點右邊
                    xPosition = ((i % horizontalCount) - (horizontalCount / 2) + 1 ) * MODULE_SIZE - MODULE_SIZE * 0.5f;
                }
            }
            else  // X是奇數
            {
                if ((i % horizontalCount) <= (horizontalCount / 2))
                {   // 落點左邊
                    xPosition = ((horizontalCount / 2) - (i % horizontalCount)) * -MODULE_SIZE;
                }
                else if ((horizontalCount / 2) == (i % horizontalCount))
                {   // 落點中間
                    xPosition = 0.0f;
                }
                else
                {   // 落點右邊
                    xPosition = ((i % horizontalCount)) - (horizontalCount / 2) * MODULE_SIZE;
                }
            }

            if (verticalCount % 2 == 0)  // z是偶數
            {
                if ((i / verticalCount) < (verticalCount / 2))
                {   // 落點上邊
                    zPosition = ((verticalCount / 2) - (i / horizontalCount)) * -MODULE_SIZE + MODULE_SIZE * 0.5f;
                }
                else
                {   // 落點下邊
                    zPosition = ((i / horizontalCount) - (verticalCount / 2) + 1) * MODULE_SIZE - MODULE_SIZE * 0.5f;
                }
            }
            else  // z是奇數
            {
                if ( (i / horizontalCount) < (verticalCount / 2))
                {   // 落點上邊
                    zPosition = ((verticalCount / 2) - (i / horizontalCount)) * -MODULE_SIZE;
                }
                else if ((verticalCount / 2) == (i / horizontalCount))
                {   // 落點中間
                    zPosition = 0.0f;
                }
                else
                {   // 落點下邊
                    zPosition = ((i / horizontalCount)) - (verticalCount / 2) * MODULE_SIZE;
                }
            }
            aModulePositionData.Add(new Vector2(i % horizontalCount + 1, i / horizontalCount + 1), new Vector2(xPosition, zPosition));
        }

    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private bool      aIsSetUpFinish;
    private LevelData aCurrentLevelData;
    private Dictionary<Vector2, ModuleBase> aModuleList         = new Dictionary<Vector2, ModuleBase>( );
    private Dictionary<Vector2, Vector2>    aModulePositionData = new Dictionary<Vector2, Vector2>();
    private Dictionary<ModuleBase, bool>    aUpdatedModule      = new Dictionary<ModuleBase, bool>();

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float MODULE_SIZE = 1.0f;

}
