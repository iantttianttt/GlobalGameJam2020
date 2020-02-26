using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "REPIPE/Create UI Panel Reference Data")]

public class UIPanelReferenceObject : ScriptableObject
{
	public List<UIPanelReference> UIPanelReferenceData = new List<UIPanelReference>();
}

[System.Serializable]
public struct UIPanelReference
{
	public EUIPanelType UIPanelType;
	public GameObject   ObjectReference;
}