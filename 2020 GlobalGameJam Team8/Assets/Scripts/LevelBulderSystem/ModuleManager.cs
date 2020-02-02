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
    public LevelData  GetCurrentLevelData { get { return aCurrentLevelData; }} //TODO
    public GameObject Floor;
    public GameObject UpWall;
    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject DownWall;
    public ModuleReferenceObject ModuleReferenceObject;
    public Dictionary<Vector2, Vector3> ModulePositionData { get { return aModulePositionData; } }

    public bool Updateing;

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

        Floor.transform.localScale = new Vector3(aCurrentLevelData.LevelLayout.x + 2, 1.0f, aCurrentLevelData.LevelLayout.y + 2);
        Vector3 upRight;
        Vector3 downLeft;
        aModulePositionData.TryGetValue(new Vector2(aCurrentLevelData.LevelLayout.x, aCurrentLevelData.LevelLayout.y), out upRight);
        aModulePositionData.TryGetValue(new Vector2(1, 1), out downLeft);
        UpWall.transform.position    = new Vector3(UpWall.transform.position.x, UpWall.transform.position.y, upRight.z + 1);
        RightWall.transform.position = new Vector3(upRight.x + 1, RightWall.transform.position.y, RightWall.transform.position.z);
        DownWall.transform.position  = new Vector3(DownWall.transform.position.x, DownWall.transform.position.y, downLeft.z - 1);
        LeftWall.transform.position  = new Vector3(downLeft.x - 1, LeftWall.transform.position.y, LeftWall.transform.position.z);

        UpWall.transform.localScale    = new Vector3(aCurrentLevelData.LevelLayout.x + 2, 1.0f, 1);
        RightWall.transform.localScale = new Vector3(1, 1.0f, aCurrentLevelData.LevelLayout.y + 2);
        LeftWall.transform.localScale  = new Vector3(1, 1.0f, aCurrentLevelData.LevelLayout.y + 2);
        DownWall.transform.localScale  = new Vector3(aCurrentLevelData.LevelLayout.x + 2, 1.0f, 1);

        foreach (var OneItem in aCurrentLevelData.DefaultModule)
        {
            RequestSpawnModule(OneItem);
        }

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
        if (!aIsSetUpFinish)                                   { return null; }
        if (iModuleIndex.y == aCurrentLevelData.LevelLayout.y) { return null; }

        ModuleBase hasValue = null;
        if (aSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x, iModuleIndex.y + 1), out hasValue))
        {
            foreach (ModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == ModuleDirection.DOWN)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    public ModuleBase GetDownModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish)     { return null; }
        if (iModuleIndex.y == 1) { return null; }

        ModuleBase hasValue = null;
        if (aSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x, iModuleIndex.y - 1), out hasValue))
        {
            foreach (ModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == ModuleDirection.UP)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    public ModuleBase GetLeftModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish)     { return null; }
        if (iModuleIndex.y == 1) { return null; }

        ModuleBase hasValue = null;
        if (aSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x - 1, iModuleIndex.y), out hasValue))
        {
            foreach (ModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == ModuleDirection.RIGHT)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    public ModuleBase GetRightModule(Vector2 iModuleIndex)
    {
        if (!aIsSetUpFinish)                                   { return null; }
        if (iModuleIndex.y == aCurrentLevelData.LevelLayout.x) { return null; }

        ModuleBase hasValue = null;
        if (aSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x + 1, iModuleIndex.y), out hasValue))
        {
            foreach (ModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == ModuleDirection.LEFT)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }



    public ModuleBase RequestSpawnModule(ModuleData iModuleData, ModuleBase iCreater = null)
    {
        foreach(ModuleReference reference in ModuleReferenceObject.ModuleReferenceData)
        {
            if (reference.ModuleType == iModuleData.ModuleType)
            {
                Vector3 SpawnPos = new Vector3(0, 0, 0);

                if (iCreater!=null)
                {
                    SpawnPos = iCreater.transform.position;
                    GameObject newObj = Instantiate(reference.ObjectReference, SpawnPos, Quaternion.identity);
                    ModuleBase newModule = newObj.GetComponent<ModuleBase>();
                    newModule.InitModule(iModuleData);
                    aAllModule.Add(newModule);
                    return newModule;
                }

                if(aModulePositionData.TryGetValue(iModuleData.SetUpIndex, out SpawnPos))
                {
                    GameObject newObj    = Instantiate(reference.ObjectReference, SpawnPos, Quaternion.identity);
                    ModuleBase newModule = newObj.GetComponent<ModuleBase>();
                    newModule.InitModule(iModuleData);
                    aSetUpModuleList.Add(iModuleData.SetUpIndex, newModule);
                    aAllModule.Add(newModule);
                    newModule.SetUpModule();
                    return newModule;
                }
            }
        }
        return null;
    }

    public void RequestDestoryModule(ModuleBase iModuleBase)
    {
        aAllModule.Remove(iModuleBase);
        Destroy(iModuleBase.gameObject);
    }

    public bool RequestModuleUpdate(ModuleBase iTargetModule)
    {
        if (!aIsSetUpFinish)       { return false; }
        if (iTargetModule == null) { return false; }

        bool hasUpdated = false;
        foreach (var updatedModule in aUpdatedModule)
        {
            if (iTargetModule == updatedModule)
            {
                hasUpdated = true;
            }
        }
        if (!hasUpdated)
        {
            iTargetModule.UpdateModule();
            aUpdatedModule.Add(iTargetModule);
            return true;
        }
        return false;
    }

    public void ModuleAutoUpdate()
    {
        if (!aIsSetUpFinish) { return; }
        aUpdatedModule.Clear();

        for (int i = 0; i < aAllModule.Count; i++)
        {
            if (aAllModule[i].AutoUpdateModule)
            {
                bool hasUpdated = false;
                for (int j = 0; j < aUpdatedModule.Count; j++)
                {
                    if (aAllModule[i] == aUpdatedModule[j])
                    {
                        hasUpdated = true;
                    }
                }
                if (!hasUpdated)
                {
                    aAllModule[i].UpdateModule();
                    aUpdatedModule.Add(aAllModule[i]);
                }
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
            aModulePositionData.Add(new Vector2(i % horizontalCount + 1, i / horizontalCount + 1), new Vector3(xPosition, MODULE_HIGH, zPosition));
        }

    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private bool      aIsSetUpFinish;
    private LevelData aCurrentLevelData;
    private Dictionary<Vector2, ModuleBase> aSetUpModuleList        = new Dictionary<Vector2, ModuleBase>( );
    private Dictionary<Vector2, Vector3>    aModulePositionData     = new Dictionary<Vector2, Vector3>();
    private List<ModuleBase>                aAllModule              = new List<ModuleBase>();
    private List<ModuleBase>                aUpdatedModule          = new List<ModuleBase>();



    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float MODULE_SIZE = 1.0f;
    private const float MODULE_HIGH = 0.0f;

}
