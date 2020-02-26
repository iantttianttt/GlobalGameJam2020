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
    public bool       GetIsSetUpFinish                     { get { return mIsSetUpFinish;      } }
    public LevelData  GetCurrentLevelData                  { get { return mCurrentLevelData;   } } 
    public Dictionary<Vector2, Vector3> ModulePositionData { get { return mModulePositionData; } }
    public Dictionary<Vector2, ModuleBase> SetUpModuleList { get { return mSetUpModuleList;    } }
    public List<ModuleBase>    UpdatingModule              { get { return mUpdatingModule;     } }
    public List<EModuleType>   GetHammerBreakableList()    { return mHammerBreakableList;        }
    public void                AddLinkCount()              { mCurrentLinkCount++;                }

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化 Module Manager
    /// </summary>
    public void InitModuleManager()
    {
        mIsSetUpFinish         = false;
        mModuleReferenceObject = Resources.Load<ModuleReferenceObject>(MODULE_REFERENCE_OBJECT_PATH);
        mHammerBreakableList   = Resources.Load<HammerBreakableList>(HAMMER_BREAKABLE_LIST_PATH).BreakableList;
        mModuleRoot            = new GameObject();
        mModuleRoot.name       = MODULE_ROOT_NAME;
        ModulePositionData.Clear();
        SetUpModuleList.Clear();
    }

    /// <summary>
    /// 設置初始模組
    /// </summary>
    public void SetupDefaultModule(LevelData iLevelData)
    {
        mCurrentLevelData = iLevelData;
        CalculateModulePositionData(mCurrentLevelData);

        //Setup DefaultModule
        foreach (var OneItem in mCurrentLevelData.DefaultModule)
        {
            RequestSpawnModule(OneItem);
        }

        mIsSetUpFinish = true;
    }
    
    //------------------------------------
    // Clear Level
    //------------------------------------
    public void ClearLevel()
    {
        
    }

    //------------------------------------
    // Reset Level
    //------------------------------------
    public void ResetLevel()
    {
        
    }

    /// <summary>
    /// 取得輸入值上方一格模組
    /// </summary>
    public ModuleBase GetUpModule(Vector2 iModuleIndex)
    {
        if (!mIsSetUpFinish)                                   { return null; }
        if (iModuleIndex.y == mCurrentLevelData.LevelLayout.y) { return null; } //已抵達邊界

        ModuleBase hasValue = null;
        if (mSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x, iModuleIndex.y + 1), out hasValue))
        {
            foreach (EModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == EModuleDirection.DOWN)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 取得輸入值下方一格模組
    /// </summary>
    public ModuleBase GetDownModule(Vector2 iModuleIndex)
    {
        if (!mIsSetUpFinish)     { return null; }
        if (iModuleIndex.y == 1) { return null; } //已抵達邊界

        ModuleBase hasValue = null;
        if (mSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x, iModuleIndex.y - 1), out hasValue))
        {
            foreach (EModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == EModuleDirection.UP)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 取得輸入值左方一格模組
    /// </summary>
    public ModuleBase GetLeftModule(Vector2 iModuleIndex)
    {
        if (!mIsSetUpFinish)     { return null; }
        if (iModuleIndex.y == 1) { return null; } //已抵達邊界

        ModuleBase hasValue = null;
        if (mSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x - 1, iModuleIndex.y), out hasValue))
        {
            foreach (EModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == EModuleDirection.RIGHT)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 取得輸入值右方一格模組
    /// </summary>
    public ModuleBase GetRightModule(Vector2 iModuleIndex)
    {
        if (!mIsSetUpFinish)                                   { return null; }
        if (iModuleIndex.y == mCurrentLevelData.LevelLayout.x) { return null; } //已抵達邊界

        ModuleBase hasValue = null;
        if (mSetUpModuleList.TryGetValue(new Vector2(iModuleIndex.x + 1, iModuleIndex.y), out hasValue))
        {
            foreach (EModuleDirection linkDir in hasValue.ModuleLinkDirection)
            {
                if (linkDir == EModuleDirection.LEFT)
                {
                    return hasValue;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 取得輸入值最近一格模組之格狀定位座標
    /// </summary>
    public Vector2 GetClosestIndexDictionary(Vector3 iCurrentPos)
    {
        Vector2 targetIndex = new Vector2(0, 0);
        float   closestDis  = 99999.0f;
        foreach (var OneItem in mModulePositionData)
        {
            float newDis = Vector3.Distance(iCurrentPos, OneItem.Value);
            if(newDis < closestDis)
            {
                targetIndex = OneItem.Key;
                closestDis  = newDis;
            }
        }
        return targetIndex;
    }

    /// <summary>
    /// 嘗試創建新模組
    /// </summary>
    public ModuleBase RequestSpawnModule(ModuleData iModuleData, ModuleBase iCreater = null)
    {
        foreach(ModuleReference reference in mModuleReferenceObject.ModuleReferenceData)
        {
            if (reference.ModuleType == iModuleData.ModuleType)
            {
                Vector3 SpawnPos = new Vector3(0, 0, 0);

                if (iCreater!=null)
                {
                    SpawnPos = iCreater.transform.position;
                    // TODO Object Pool
                    GameObject newObj = ObjectPool.Instance.Spawn(reference.ObjectReference, SpawnPos, Quaternion.identity, mModuleRoot.transform);
                    ModuleBase newModule = newObj.GetComponent<ModuleBase>();
                    newModule.InitModule(iModuleData);
                    mAllModule.Add(newModule);
                    return newModule;
                }

                if(mModulePositionData.TryGetValue(iModuleData.SetUpIndex, out SpawnPos))
                {
                    // TODO Object Pool
                    GameObject newObj    = ObjectPool.Instance.Spawn(reference.ObjectReference, SpawnPos, Quaternion.identity, mModuleRoot.transform);
                    ModuleBase newModule = newObj.GetComponent<ModuleBase>();
                    newModule.InitModule(iModuleData);
                    mAllModule.Add(newModule);
                    newModule.SetUpModule(iModuleData.SetUpIndex,true);
                    if (newModule.ModuleType != EModuleType.PLAYER_SPAWN_POINT) //避免玩家生成位置無法放置其他模組
                    {
                        mSetUpModuleList.Add(iModuleData.SetUpIndex, newModule);
                    }
                    if(newModule.ModuleType == EModuleType.TUBE_START) //儲存起始水管
                    {
                        mModuleTube_Start = (ModuleTube_Start)newModule;
                    }
                    return newModule;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 嘗試刪除模組
    /// </summary>
    public void RequestDestoryModule(ModuleBase iModuleBase)
    {
        mAllModule.Remove(iModuleBase);
        mSetUpModuleList.Remove(iModuleBase.ModuleIndex);
        ObjectPool.Instance.Despawn(iModuleBase.gameObject);
    }

    /// <summary>
    /// 刪除已更新模組清單
    /// </summary>
    public void CleanModuleUpdateList()
    {
        mUpdatedModule.Clear();
    }

    /// <summary>
    /// 確認有效連接水管數
    /// </summary>
    public void StartCheckLinkedCount()
    {
        if (!mIsSetUpFinish)      { return; }
        mLastFrameLinkCount = mCurrentLinkCount;
        mCurrentLinkCount   = 0;
        RequestModuleUpdate(mModuleTube_Start);
    }

    /// <summary>
    /// 確認資料檢查完畢
    /// </summary>
    public bool CheckIsLinkCheckOver()
    {
        if (!mIsSetUpFinish)      { return false; }
        if (mUpdatingModule.Count == 0)
        {
            if(mCurrentLinkCount > mLastFrameLinkCount)
            {
                GameController.Instance.ResetPressureTimer();
            }
            //Debug.Log("Cur: " + mCurrentLinkCount + "   Last: " + mLastFrameLinkCount);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 嘗試更新模組
    /// </summary>
    public bool RequestModuleUpdate(ModuleBase iTargetModule)
    {
        if (!mIsSetUpFinish)       { return false; }
        if (iTargetModule == null) { return false; }

        bool hasUpdated = false;
        foreach (var updatedModule in mUpdatedModule)
        {
            if (iTargetModule == updatedModule)
            {
                hasUpdated = true;
            }
        }
        if (!hasUpdated)
        {
            mUpdatedModule.Add(iTargetModule);
            iTargetModule.UpdateModule();
            if(mUpdatingModule.Contains(iTargetModule))
            {
                mUpdatingModule.Remove(iTargetModule);
            }
            return true;
        }
        //Module has been updated this frame.
        if(mUpdatingModule.Contains(iTargetModule))
        {
            mUpdatingModule.Remove(iTargetModule);
        }
        return false;
    }

    /// <summary>
    /// 更新自動更新模組
    /// </summary>
    public void ModuleAutoUpdate()
    {
        if (!mIsSetUpFinish) { return; }

        for (int i = mAllModule.Count - 1; i >= 0; i--)
        {
            if (mAllModule[i].AutoUpdateModule)
            {
                if(mUpdatedModule.Contains(mAllModule[i])) { continue; }
                mAllModule[i].UpdateModule();
                mUpdatedModule.Add(mAllModule[i]);
            }
        }

        foreach (ModuleBase module in mDestoryRequestModules)
        {
            if (module != null) { RequestDestoryModule(module); }
        }
        mDestoryRequestModules.Clear();
    }

    /// <summary>
    /// 儲存將刪除的模組，統一在自動更新結束後刪除，以避免自動更新中陣列錯誤
    /// </summary>
    public void AddDestoryRequestModule(ModuleBase _Module) 
    { 
        mDestoryRequestModules.Add(_Module);
    }


    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 計算並取得所有格狀座標位置
    /// </summary>
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
            mModulePositionData.Add(new Vector2(i % horizontalCount + 1, i / horizontalCount + 1), new Vector3(xPosition, MODULE_HIGH, zPosition));
        }

    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private bool                            mIsSetUpFinish;
    private LevelData                       mCurrentLevelData;
    private Dictionary<Vector2, ModuleBase> mSetUpModuleList        = new Dictionary<Vector2, ModuleBase>( );
    private Dictionary<Vector2, Vector3>    mModulePositionData     = new Dictionary<Vector2, Vector3>();
    private List<ModuleBase>                mAllModule              = new List<ModuleBase>();
    private List<ModuleBase>                mUpdatedModule          = new List<ModuleBase>();
    private ModuleReferenceObject           mModuleReferenceObject;
    private List<EModuleType>               mHammerBreakableList;

    private ModuleTube_Start                mModuleTube_Start;
    private int                             mCurrentLinkCount;
    private int                             mLastFrameLinkCount;
    private List<ModuleBase>                mUpdatingModule          = new List<ModuleBase>();
    private GameObject                      mModuleRoot;
    private List<ModuleBase>                mDestoryRequestModules   = new List<ModuleBase>();

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float  MODULE_SIZE = 1.0f;
    private const float  MODULE_HIGH = 0.0f;
    private const string MODULE_REFERENCE_OBJECT_PATH = "GameSetting/Module Reference Data";
    private const string HAMMER_BREAKABLE_LIST_PATH   = "GameSetting/Hammer Breakable List";
    private const string MODULE_ROOT_NAME             = "ModuleRoot";

}
