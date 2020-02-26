using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化 Module Manager
    /// </summary>
    public void InitUIManager()
    {
        SetPanelDataList();
        mIsInit = true;
    }

    /// <summary>
    /// 顯示介面
    /// </summary>
    public IUIPanel ShowPanel(EUIPanelType _UIPanelType, IUIPanelData _PanelData = null)
    {
        if (!mIsInit)
        {
            Debug.LogError("UI Manager not Init yet!");
            return null;
        }
        //介面已經在顯示中
        if (IsPanelOnShow(_UIPanelType))
        {
            Debug.LogErrorFormat("[{0}] is Showing, if you want to show, please close first!!", _UIPanelType);
            return null;
        }
        //介面已經生成，但目前未顯示
        if (HasPanel(_UIPanelType))
        {
            IUIPanel panel;
            if (mSpawnPanelList.TryGetValue(_UIPanelType, out panel))
            {
                panel.ShowPanel(_PanelData);
                return panel;
            }
            return null;
        }
        //介面未生成，生成新介面
        else
        {
            GameObject loadGo = null;
            if (mPanelDataList.TryGetValue(_UIPanelType, out loadGo))
            {
                GameObject panelObj = Utility.GameObjectRelate.InstantiateGameObject(GetCanvasRoot(), loadGo);
                mSpawnPanelList.Add(_UIPanelType, panelObj.GetComponent<IUIPanel>());
                panelObj.GetComponent<IUIPanel>().ShowPanel(_PanelData);
                return panelObj.GetComponent<IUIPanel>();
            }
            Debug.LogErrorFormat("This [{0}] Type Can't Find Prefab Reference in UI Panel Reference Object", _UIPanelType);
            return null;
        }
    }

    /// <summary>
    /// 取得介面 介面在Show之後才可取得
    /// </summary>
    public IUIPanel GetUIPanel(EUIPanelType _UIPanelType)
    {
        if (!IsPanelOnShow(_UIPanelType)) { return null; }
        IUIPanel panel;
        if (mSpawnPanelList.TryGetValue(_UIPanelType, out panel))
        {
            return panel;
        }
        return null;
    }

    /// <summary>
    /// 隱藏介面
    /// </summary>
    public void HidePanel(EUIPanelType _UIPanelType, IUIPanelData _PanelData = null)
    {
        if (!IsPanelOnShow(_UIPanelType)) { return; }
        IUIPanel panel;
        if (mSpawnPanelList.TryGetValue(_UIPanelType, out panel))
        {
            panel.HidePanel(_PanelData);
        }
    }

    /// <summary>
    /// 清除介面
    /// </summary>
    public void ClearPanel(EUIPanelType _UIPanelType)
    {
        if (HasPanel(_UIPanelType))
        {
            if (mSpawnPanelList[_UIPanelType] != null)
            {
                mSpawnPanelList[_UIPanelType].OnDeletePanel();
                Object.Destroy(mSpawnPanelList[_UIPanelType].gameObject);
            }
            mSpawnPanelList.Remove(_UIPanelType);
        }
        else
        {
            Debug.LogErrorFormat("ClosePanel [{0}] not found.", name);
        }
    }
    /// <summary>
    /// 清除所有介面
    /// </summary>
    public void ClearAllPanel()
    {
        foreach (KeyValuePair<EUIPanelType, IUIPanel> item in mSpawnPanelList)
        {
            if (item.Value != null)
            {
                item.Value.OnDeletePanel();
                Object.Destroy(item.Value.gameObject);
            }
        }
        mSpawnPanelList.Clear();
    }
/*
    public Vector2 GetCanvasSize()
    {
        if (CheckCanvasRootIsNull())
        {
            return Vector2.one * -1;
        }
        RectTransform trans = m_CanvasRoot.transform as RectTransform;

        return trans.sizeDelta;
    }
*/
    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    private void SetPanelDataList()
    {
        mPanelDataList.Clear();
        List<UIPanelReference> UIPanelReferenceData = Utility.AssetRelate.ResourcesLoadCheckNull<UIPanelReferenceObject>(UI_PANEL_REFERENCE_OBJECT_PATH).UIPanelReferenceData;
        foreach (UIPanelReference reference in UIPanelReferenceData)
        {
            mPanelDataList.Add(reference.UIPanelType, reference.ObjectReference);
        }
    }
    private GameObject GetCanvasRoot()
    {
        if (mCanvasRoot == null)
        {
            GameObject canvasRootRef = Utility.AssetRelate.ResourcesLoadCheckNull<GameObject>(UI_CANVAS_ROOT_OBJECT_PATH);
            mCanvasRoot              = Utility.GameObjectRelate.InstantiateGameObject(this.gameObject, canvasRootRef);
        }
        return mCanvasRoot;
    }

    private bool HasPanel(EUIPanelType _UIPanelType)
    {
        return mSpawnPanelList.ContainsKey(_UIPanelType);
    }

    private bool IsPanelOnShow(EUIPanelType _UIPanelType)
    {
        if (HasPanel(_UIPanelType))
        {
            return mSpawnPanelList[_UIPanelType].gameObject.activeSelf;
        }
        return false;
    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private bool       mIsInit                                   = false;
    private GameObject mCanvasRoot;
    private Dictionary<EUIPanelType, IUIPanel>   mSpawnPanelList = new Dictionary<EUIPanelType, IUIPanel>();
    private Dictionary<EUIPanelType, GameObject> mPanelDataList  = new Dictionary<EUIPanelType, GameObject>();

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const string UI_PANEL_REFERENCE_OBJECT_PATH = "GameSetting/UI Panel Reference Object";
    private const string UI_CANVAS_ROOT_OBJECT_PATH     = "UI/CanvasRoot";
}
