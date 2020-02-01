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
    /// 玩家控制器
    /// </summary>
    public XboxController controller;

    public PlayerInfo(XboxController _controller)
    {
        controller = _controller;
    }
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
    public void AddPlayer(PlayerInfo player)
    {
        players.Add(player);
    }   

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(PlayerInfo player)
    {
        players.Remove(player);
    }

}
