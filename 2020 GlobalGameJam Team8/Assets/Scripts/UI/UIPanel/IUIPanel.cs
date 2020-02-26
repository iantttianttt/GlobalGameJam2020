using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IUIPanelData
{

}


[System.Serializable]
public class IUIPanel : MonoBehaviour
{
	//-----------------------------------------------------------------------
	//Public Parameter
	//-----------------------------------------------------------------------

	//-----------------------------------------------------------------------
	// Get
	//-----------------------------------------------------------------------
	public EUIPanelType PanelType     { get { return mUIPanelType;   } set { mUIPanelType = value; } }
	public bool         StatePrepared { get { return mStatePrepared; }                               }

	// 開始
	public virtual void ShowPanel(IUIPanelData _PanelData)
	{
		this.gameObject.SetActive(true);
	}

	// 結束
	public virtual void HidePanel(IUIPanelData _PanelData)
	{
		this.gameObject.SetActive(false);
	}

	// 更新
	public virtual void OnDeletePanel()
	{ }

	public override string ToString()
	{
		return string.Format("[IGameState: StateName={0}]", mUIPanelType);
	}

	//-----------------------------------------------------------------------
	// Private Parameter
	//-----------------------------------------------------------------------
	protected EUIPanelType mUIPanelType   = EUIPanelType.NONE;
	protected bool         mStatePrepared = false;

}

