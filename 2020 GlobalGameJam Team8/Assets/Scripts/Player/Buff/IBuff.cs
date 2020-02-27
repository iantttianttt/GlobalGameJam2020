using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBuff
{
    public float lifeTime = 1f;
    public PlayerDetail buffDetail;
    public bool isStart=false;

    protected IBuff(float _lifeTime) 
    {
        buffDetail = new PlayerDetail();
        lifeTime = _lifeTime; 
    }
    public virtual void Enter(){ isStart = true; }
    public virtual void Update(){}
    public virtual void Exit(){}

}

public class Dizziness : IBuff
{
    public Dizziness(float _lifeTime) : base(_lifeTime) { }

    public override void Enter()
    {
        buffDetail.speed = -Mathf.Infinity;
        buffDetail.addSpeed = -Mathf.Infinity;
        base.Enter();
    }
    public override void Exit()
    {
        Debug.Log("DizzinessDone");
    }
}


public class DeSpeed : IBuff
{
    public DeSpeed(float _lifeTime) : base(_lifeTime) { }
    public override void Enter()
    {
        buffDetail.speed = -2;
        base.Enter();
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
        Debug.Log("DeSpeedDone");
    }
}
