using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleBase : MonoBehaviour
{

    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------
    public Vector2                ModuleIndex         { get { return mModuleIndex;     } set { mModuleIndex     = value; } }
    public EModuleDirection       ModuleDirection     { get { return mModuleDirection; } set { mModuleDirection = value; } }
    public EModuleType            ModuleType          { get { return mModuleType;      } set { mModuleType      = value; } }
    public List<EModuleDirection> ModuleLinkDirection { get { return mModuleLinkDirection;                               } }
    public bool                   AutoUpdateModule    { get { return mAutoUpdate;                                        } }
    public bool                   IsOnConveyor        { get { return mIsOnConveyor;    } set { mIsOnConveyor    = value; } }
    public float                  ConveyorSpeed       { get { return mConveyorSpeed;   } set { mConveyorSpeed   = value; } }

    public void AddConveyorTarget (Vector3 iNewTarget) { mConveyorMoveTarget.Add(iNewTarget); }


    //----------------------------------------------------------------------- 
    //Public virtual Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化模組，在生成模組時使用
    /// </summary>
    public virtual void InitModule(ModuleData iModuleData)
    {
        mModuleDirection = iModuleData.ModuleDirection;
        mModuleIndex     = iModuleData.SetUpIndex;
        mModuleType      = iModuleData.ModuleType;
        switch (mModuleDirection)
        {
            case EModuleDirection.DOWN:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f));
                break;
            case EModuleDirection.RIGHT:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));
                break;
            case EModuleDirection.LEFT:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f));
                break;
        }
    }

    /// <summary>
    /// 鎖定模組，在將模組設定到場景中時使用
    /// </summary>
    public virtual void SetUpModule(Vector2 iNewIndex , bool iIsDefaultSetUp = false)
    {
        if (iIsDefaultSetUp) { return; }

        ModuleBase moduleBase;
        ModuleManager.Instance.SetUpModuleList.TryGetValue(iNewIndex, out moduleBase);
        if (moduleBase == null)
        {
            ModuleManager.Instance.SetUpModuleList.Add(iNewIndex, this);
        }
        mModuleIndex = iNewIndex;
    }

    /// <summary>
    /// 更新模組，觸發模組功能
    /// </summary>
    public virtual void UpdateModule()
    {
        if (mIsOnConveyor && mConveyorMoveTarget.Count != 0)
        {
            this.gameObject.GetComponent<Collider>().isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position, mConveyorMoveTarget[0], Time.deltaTime * mConveyorSpeed);
            if (Vector3.Distance(transform.position, mConveyorMoveTarget[0]) < STOP_CONVEYOR_MOVE_DISTANCE)
            {
                if (mConveyorMoveTarget.Count >= 2)
                {
                    mConveyorMoveTarget.Remove(mConveyorMoveTarget[0]);
                }
                else if(mToDestory)
                {
                    ModuleManager.Instance.RequestDestoryModule(this);
                }
                else
                {
                    mIsOnConveyor = false;
                }
            }
        }
    }

    /// <summary>
    /// 刪除模組
    /// </summary>
    public virtual void RemoveModule()
    {
        Destroy(this.gameObject);
    }

    //----------------------------------------------------------------------- 
    // Virtual Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 從輸送帶上取下模組
    /// </summary>
    public void TakeOutModuleFromConveyor()
    {
        mIsOnConveyor = false;
        mAutoUpdate   = false;
    }

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        ModuleConveyor moduleConveyor = other.gameObject.GetComponent<ModuleConveyor>();
        if (moduleConveyor != null && IsOnConveyor)
        {
            Vector3 conveyorTarget = new Vector3(0.0f, 0.0f, 0.0f);
            switch (moduleConveyor.ModuleDirection)
            {
                case EModuleDirection.DOWN_TO_UP:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x, moduleConveyor.ModuleIndex.y + 1), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case EModuleDirection.UP_TO_DOWN:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x, moduleConveyor.ModuleIndex.y - 1), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case EModuleDirection.RIGHT_TO_LEFT:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x - 1, moduleConveyor.ModuleIndex.y), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case EModuleDirection.LEFT_TO_RIGHT:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x + 1, moduleConveyor.ModuleIndex.y), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
            }
        }

        Module_Cleaner module_Cleaner = other.gameObject.GetComponent<Module_Cleaner>();
        if (module_Cleaner != null && IsOnConveyor)
        {
            mConveyorMoveTarget.Clear();
            AddConveyorTarget(module_Cleaner.transform.position);
            mToDestory = true;
        }
    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------

    protected Vector2               mModuleIndex;
    protected EModuleDirection       mModuleDirection;
    protected EModuleType            mModuleType;
    protected List<EModuleDirection> mModuleLinkDirection;
    protected bool                  mAutoUpdate         = true;
    protected bool                  mIsOnConveyor;
    protected List<Vector3>         mConveyorMoveTarget = new List<Vector3>();
    protected float                 mConveyorSpeed;
    /// <summary>
    /// 正在朝向輸送帶的終點刪除器移動
    /// </summary>
    protected bool                  mToDestory          = false;

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float STOP_CONVEYOR_MOVE_DISTANCE = 0.05f;
    private const float MODULE_ON_CONVEYOR_HIGH     = 0.5f;

}
