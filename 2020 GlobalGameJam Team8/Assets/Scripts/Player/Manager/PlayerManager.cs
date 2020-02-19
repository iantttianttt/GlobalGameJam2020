using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XboxCtrlrInput;

/// <summary>
/// 玩家控制器資訊
/// </summary>
public class PlayerInfo
{
    /// <summary>
    /// 玩家是否加入遊戲
    /// </summary>
    public bool inGame = false;

    /// <summary>
    /// 玩家是否被創建進遊戲中
    /// </summary>
    public bool isSpawned=false;

    /// <summary>
    /// 玩家控制器種類
    /// </summary>
    public ControllerType controllerType;

    public ColorType color;

    public PlayerInfo(ControllerType _controllerType, ColorType _color)
    {
        controllerType = _controllerType;
        color = _color;
    }
}

public enum ColorType { 
    Red=0,
    Blue=1,
    Yellow=2,
    Green=3
}

public enum ControllerType
{    
    Xbox_First,
    Xbox_Second,
    Xbox_Third,
    Xbox_Fourth,
    Keyboard1,
    Keyboard2
}

/// <summary>
/// 玩家管理員
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    /// <summary>
    /// 所有玩家物件
    /// </summary>
    public List<PlayerInfo> players;

    /// <summary>
    /// 建構子
    /// </summary>
    public PlayerManager()
    {
        players = new List<PlayerInfo>();
    }

    /// <summary>
    /// 新增玩家
    /// </summary>
    /// <param name="player"></param>
    public void AddPlayer(ControllerType _controllerType)
    {
        ColorType _color;

        if(players.Find((x) => x.color == ColorType.Red)==null)
            _color = ColorType.Red;
        else if(players.Find((x) => x.color == ColorType.Yellow) == null)
            _color = ColorType.Yellow;        
        else if(players.Find((x) => x.color == ColorType.Green) == null)
            _color = ColorType.Green;        
        else
            _color = ColorType.Blue;
        players.Add(new PlayerInfo(_controllerType, _color));

    }   

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(PlayerInfo player)
    {
        players.Remove(player);
    }

    public PlayerInfo SearchPlayer(ControllerType _controllerType)
    {
        return players.Find((x) => x.controllerType == _controllerType);
    }
}
