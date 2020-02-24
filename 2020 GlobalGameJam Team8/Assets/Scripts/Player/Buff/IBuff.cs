using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBuff
{
    public float lifeTime = 1f;
    public Player player;
    public bool isStart=false;

    protected IBuff() { }
    public virtual void Enter(){ isStart = true; }
    public virtual void Update(){}
    public virtual void Exit(){}

}

public class Dizziness : IBuff
{
    public Dizziness() : base() { }
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
    public DeSpeed() : base() { }
    float deSpeed=3;
    public override void Enter()
    {
        lifeTime = 5;
        if (player.speed - deSpeed <= 0)
        {
            deSpeed = player.speed;
            player.speed = 0;            
        }

        base.Enter();
    }
    public override void Exit()
    {
        player.speed +=deSpeed;
    }
}


