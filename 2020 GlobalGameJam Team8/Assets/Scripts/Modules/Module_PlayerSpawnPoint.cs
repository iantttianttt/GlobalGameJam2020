using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module_PlayerSpawnPoint : ModuleBase
{
	public override void InitModule(ModuleData iModuleData)
	{
		base.InitModule(iModuleData);

		GameManager.Instance.AddPlayerSqawnPos(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z));
	}

}
