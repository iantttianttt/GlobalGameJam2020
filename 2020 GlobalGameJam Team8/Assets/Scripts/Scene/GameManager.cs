using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;
    public MainUI mainUI;
    public Vector2[] playerSqawnPos;

    private float pressure = 0;
    private float pressureScale = 0.1f;

    /// <summary>
    /// 設置玩家生成位置
    /// </summary>
    /// <param name="_sqawnPos"></param>
    public void SetPlayerSqawnPos(Vector2[] _sqawnPos)
    {
        playerSqawnPos = _sqawnPos;
    }


    void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        for (int i=0;i < PlayerManager.Instance.players.Count; i++)
        {
            Instantiate(playerPrefab,new Vector3(playerSqawnPos[i].x,1, playerSqawnPos[i].y), playerPrefab.transform.rotation);            
        }
    }

    private void Update()
    {
        pressure += Time.deltaTime * pressureScale;
        mainUI.SetBar(pressure);

    }
}
