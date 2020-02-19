using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GlobalGameJam2020/Create ModuleReferenceData")]

public class ModuleReferenceObject : ScriptableObject
{
	public List<ModuleReference> ModuleReferenceData = new List<ModuleReference>();
}

[System.Serializable]
public struct ModuleReference
{
	public EModuleType ModuleType;
	public GameObject ObjectReference;
}