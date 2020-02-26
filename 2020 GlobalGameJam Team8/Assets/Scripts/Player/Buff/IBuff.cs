using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBuff
{
    public float lifeTime = 1f;
    public Player player;
    public bool isStart=false;

    protected IBuff(float _lifeTime) { lifeTime = _lifeTime; }
    public virtual void Enter(){ isStart = true; }
    public virtual void Update(){}
    public virtual void Exit(){}

}

public class Dizziness : IBuff
{
    public Dizziness(float _lifeTime) : base(_lifeTime) { }
    float deSpeed;
    public override void Enter()
    {
        deSpeed = player.speed;
        player.speed -= deSpeed;
        base.Enter();
    }
    public override void Exit()
    {
        player.speed += deSpeed;
    }
}


public class DeSpeed : IBuff
{
    public DeSpeed(float _lifeTime) : base(_lifeTime) { }
    private float deSpeed=3;
    private float lastSpeed;
    public override void Enter()
    {
        if (player.speed<= deSpeed)
        {
            deSpeed = player.speed;
            player.speed = 0;
        }
        else
        {            
            player.speed -= deSpeed;
        }
        lastSpeed = player.speed;
        base.Enter();
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
       player.speed += deSpeed;
       
    }
}
