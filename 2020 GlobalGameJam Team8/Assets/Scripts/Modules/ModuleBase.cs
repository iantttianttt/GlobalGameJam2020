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
    public Vector2         ModuleIndex      { get { return aModuleIndex;     } set { aModuleIndex = value; } }
    public ModuleDirection ModuleDirection  { get { return aModuleDirection; } set { aModuleDirection = value; } }

    //-----------------------------------------------------------------------
    //Public Function
    //-----------------------------------------------------------------------
    public virtual void InitModule()
    {
        aModuleIndex = new Vector2(0, 0);
    }

    public virtual void SetUpModule()
    {
        aIsModuleEnable = true;
    }

    public virtual void UpdateModule()
    {
        if (aIsOnConveyor)
        {
            //Move Module
        }

        if (!aIsModuleEnable) { return; }
    }
    public virtual void RemoveModule()
    {
        Destroy(this.gameObject);
    }


    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    protected bool            aIsOnConveyor;
    protected bool            aIsModuleEnable;
    protected Vector2         aModuleIndex;
    protected ModuleDirection aModuleDirection;
    protected bool            aAutoUpdate = false;
    protected ModuleConveyor  aCurrentModuleConveyor;
    protected ModuleConveyor  aNextModuleConveyor;


}
