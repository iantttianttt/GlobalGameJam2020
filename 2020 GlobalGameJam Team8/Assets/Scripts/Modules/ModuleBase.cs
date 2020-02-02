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
    public bool            AutoUpdateModule { get { return aAutoUpdate; } }
    public bool            IsOnConveyor     { get { return aIsOnConveyor;    } set { aIsOnConveyor = value; } }
    public float           ConveyorSpeed    { get { return aConveyorSpeed;   } set { aConveyorSpeed = value; } }
    public Vector2         ModuleIndex      { get { return aModuleIndex;     } set { aModuleIndex = value; } }
    public ModuleDirection ModuleDirection  { get { return aModuleDirection; } set { aModuleDirection = value; } }
    public void AddConveyorTarget (Vector3 iNewTarget) { aConveyorTarget.Add(iNewTarget); }

    //-----------------------------------------------------------------------
    //Public Function
    //-----------------------------------------------------------------------
    public virtual void InitModule(ModuleData iModuleData)
    {
        aModuleDirection = iModuleData.ModuleDirection;
        aModuleIndex     = iModuleData.SetUpIndex;
        switch (aModuleDirection)
        {
            case ModuleDirection.DOWN:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 180f, 0.0f));
                break;
            case ModuleDirection.RIGHT:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 90f, 0.0f));
                break;
            case ModuleDirection.LEFT:
                this.gameObject.transform.Rotate(new Vector3(0.0f, 270f, 0.0f));
                break;
        }
    }

    public virtual void SetUpModule()
    {

    }

    public virtual void UpdateModule()
    {
        if (aIsOnConveyor && aConveyorTarget.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, aConveyorTarget[0], Time.deltaTime * aConveyorSpeed);
            if (Vector3.Distance(transform.position, aConveyorTarget[0]) < STOP_CONVEYOR_MOVE_DISTANCE)
            {
                if (aConveyorTarget.Count >= 2)
                {
                    aConveyorTarget.Remove(aConveyorTarget[0]);
                }
                else if(aToDestory)
                {
                    ModuleManager.Instance.RequestDestoryModule(this);
                }
                else
                {
                    aIsOnConveyor = false;
                }
            }
        }
    }
    public virtual void RemoveModule()
    {
        Destroy(this.gameObject);
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
                case ModuleDirection.DOWN_TO_UP:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x, moduleConveyor.ModuleIndex.y + 1), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case ModuleDirection.UP_TO_DOWN:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x, moduleConveyor.ModuleIndex.y - 1), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case ModuleDirection.RIGHT_TO_LEFT:
                    if (ModuleManager.Instance.ModulePositionData.TryGetValue(new Vector2(moduleConveyor.ModuleIndex.x - 1, moduleConveyor.ModuleIndex.y), out conveyorTarget))
                    {
                        AddConveyorTarget(new Vector3(conveyorTarget.x, MODULE_ON_CONVEYOR_HIGH, conveyorTarget.z));
                    }
                    break;
                case ModuleDirection.LEFT_TO_RIGHT:
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
            AddConveyorTarget(module_Cleaner.transform.position);
            aConveyorTarget.Remove(aConveyorTarget[0]);
            aToDestory = true;
        }
    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------

    protected Vector2         aModuleIndex;
    protected ModuleDirection aModuleDirection;
    protected bool            aAutoUpdate = true;

    protected bool            aIsOnConveyor;
    protected List<Vector3>   aConveyorTarget = new List<Vector3>();
    protected float           aConveyorSpeed;
    protected bool            aToDestory      = false;

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float STOP_CONVEYOR_MOVE_DISTANCE = 0.05f;
    private const float MODULE_ON_CONVEYOR_HIGH     = 0.5f;

}
