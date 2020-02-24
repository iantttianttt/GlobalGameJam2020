using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerState
{
    protected Player player;

    protected IPlayerState(Player _player){   player = _player;  }

    public virtual void Enter(){}
    
    public virtual void UpdateFrame(){}

    public virtual void Exit(){}
}

public class IdleState : IPlayerState
{
    public IdleState(Player _player) : base(_player) { }

    public override void UpdateFrame()
    {
        /// 檢查是否可以撿取物件
        Ray ray = new Ray();
        ray.origin = new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z);
        ray.direction = player.transform.forward;
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, player.handLength, 1 << 8))
        {
            if (player.controller.PressButtonA())
            {
                player.holdModule = raycastHit.transform.gameObject;
                ModuleBase moduleBase = raycastHit.transform.gameObject.GetComponent<ModuleBase>();
                if (moduleBase != null)
                {
                    moduleBase.TakeOutModuleFromConveyor();
                }
                player.putDisplay.gameObject.SetActive(true);
                player.ChangeState(new HoldingModule(player));
            }
        }
    }
}

public class HoldingModule : IPlayerState
{
    public HoldingModule(Player _player) : base(_player) { }

    public override void UpdateFrame()
    {
        player.holdModule.gameObject.transform.position = player.holdPoint.transform.position;

        Vector3 putDownPos = player.transform.position + player.transform.forward * player.handLength;
        Vector2 targetIndex = ModuleManager.Instance.GetClosestIndexDictionary(putDownPos);
        Vector3 newPutDownPos;
        ModuleManager.Instance.ModulePositionData.TryGetValue(targetIndex, out newPutDownPos);
        ModuleBase moduleBase = null;
        ModuleManager.Instance.SetUpModuleList.TryGetValue(targetIndex, out moduleBase);



        if (player.holdModule.gameObject.GetComponent<ModuleBase>().ModuleType != EModuleType.HAMMER ? moduleBase == null : moduleBase != null)
            player.putDisplay.CanPut();
        else
            player.putDisplay.CanNotPut();

        player.putDisplay.transform.position = new Vector3(newPutDownPos.x, 0, newPutDownPos.z);
        player.putDisplay.transform.eulerAngles = Vector3.zero;

        if (player.controller.PressButtonA())
        {
            bool result = PutDownModule();
            if (result)
            {
                player.ChangeState(new IdleState(player));
                player.putDisplay.DisablePutDisplay();
            }
        }

        if (player.controller.PressButtonB())
        {
            ThrowModule();
            player.ChangeState(new IdleState(player));
            player.putDisplay.DisablePutDisplay();
        }
    }


    /// 放下物件
    private bool PutDownModule()
    {
        Vector3 putDownPos = player.transform.position + player.transform.forward * player.handLength;
        Vector2 targetIndex = ModuleManager.Instance.GetClosestIndexDictionary(putDownPos);
        ModuleBase moduleBase = null;
        ModuleManager.Instance.SetUpModuleList.TryGetValue(targetIndex, out moduleBase);
        if (moduleBase == null && player.holdModule.gameObject.GetComponent<ModuleBase>().ModuleType != EModuleType.HAMMER)
        {
            Vector3 newPutDownPos;
            ModuleManager.Instance.ModulePositionData.TryGetValue(targetIndex, out newPutDownPos);
            player.holdModule.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            player.holdModule.gameObject.transform.position = new Vector3(newPutDownPos.x, 0.0f, newPutDownPos.z);
            player.holdModule.gameObject.layer = 9;
            player.holdModule.gameObject.GetComponent<Collider>().isTrigger = false;
            player.holdModule.gameObject.GetComponent<Rigidbody>().useGravity = false;
            player.holdModule.gameObject.GetComponent<ModuleBase>().SetUpModule(targetIndex);
            //GameManager.Instance.ResetPressureTimer();  //GGJ臨時用
            return true;
        }
        else if (moduleBase != null && player.holdModule.gameObject.GetComponent<ModuleBase>().ModuleType == EModuleType.HAMMER)
        {
            if (ModuleManager.Instance.GetHammerBreakableList().Contains(moduleBase.ModuleType))
            {
                ModuleManager.Instance.RequestDestoryModule(moduleBase);
                ModuleManager.Instance.RequestDestoryModule(player.holdModule.gameObject.GetComponent<ModuleBase>());
                return true;
            }
        }
        return false;
    }

    /// 拋物件
    private void ThrowModule()
    {
        if (player.holdModule)
        {
            player.holdModule.gameObject.GetComponent<Collider>().isTrigger = false;
            player.holdModule.gameObject.GetComponent<Rigidbody>().useGravity = true;
            player.holdModule.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.holdModule.gameObject.GetComponent<Rigidbody>().AddForce(((player.transform.forward + new Vector3(0, 0.7f, 0))) * player.throwPower);
        }
    }
}