using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GlobalGameJam2020/Create HammerBreakableList")]
public class HammerBreakableList : ScriptableObject
{
    public List<EModuleType> BreakableList = new List<EModuleType>();
}
