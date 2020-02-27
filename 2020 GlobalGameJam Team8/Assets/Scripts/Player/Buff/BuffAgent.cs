using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAgent
{
    private List<IBuff> Buffs;
    private Player player;

    public BuffAgent(Player _player)
    {
        player = _player;
        Init();
    }

    private void Init()
    {
        Buffs = new List<IBuff>();
    }

    public void BuffUpdate()
    {
        PlayerDetail BuffsDetail = new PlayerDetail();
        for (int i = 0; i < Buffs.Count; i++)
        {
            if (!Buffs[i].isStart) Buffs[i].Enter();
            if (Buffs[i].lifeTime<=0) 
            {
                Buffs[i].Exit();
                Buffs.Remove(Buffs[i]);
            }
            else 
            {
                Buffs[i].lifeTime -= Time.deltaTime;
                Buffs[i].Update();
            }

            BuffsDetail += Buffs[i].buffDetail;
        }

        player.Detail = player.DefaultDetail + BuffsDetail;
        player.Detail.Clamp(0, Mathf.Infinity);
    }

    public void AddBuff(IBuff addBuff)
    {
        Buffs.Add(addBuff);
    } 
    
    public void DeleteBuff(IBuff deleteBuff)
    {
        Buffs.Remove(deleteBuff);
    }

}

