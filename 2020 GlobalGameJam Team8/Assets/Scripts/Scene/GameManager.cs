using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Preparing,
    ReadyTimer,
    FirstSteamTime,
    CoreGameTime,
    GameOver,
}


public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;
    public MainUI mainUI;

    public LevelDataObject[] LevelDatas;


    /// <summary>
    /// 設置玩家生成位置
    /// </summary>
    /// <param name="_sqawnPos"></param>
    public void AddPlayerSqawnPos(Vector2 _sqawnPos)
    {
        playerSqawnPos.Add(_sqawnPos);
    }


    void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        InitNewGame();
    }

    private void Update()
    {
        pressure += Time.deltaTime * pressureScale;
 //       mainUI.SetBar(pressure);

        switch (aGameState)
        {
            case GameState.Preparing:
                break;
            case GameState.ReadyTimer:
                break;
            case GameState.FirstSteamTime:
                ModuleManager.Instance.ModuleAutoUpdate();
                aPressureTimer += Time.deltaTime;
                if (aPressureTimer > aSteamBreakTimer)
                {
                    GameOver();
                }
                break;
            case GameState.CoreGameTime:
                ModuleManager.Instance.ModuleAutoUpdate();
                aPressureTimer += Time.deltaTime;
                if(aPressureTimer > aSteamBreakTimer)
                {
                    GameOver();
                }
                break;
            case GameState.GameOver:
                break;
        }
    }


    private void InitNewGame()
    {
        aGameState = GameState.Preparing;
        InitLevel();
        SpawnPlayer();
        GameStart();
    }

    private void InitLevel()
    {
        ModuleManager.Instance.BuildLevel(LevelDatas[0].LevelData);
    }

    private void SpawnPlayer()
    {
        if (PlayerManager.Instance.players.Count <= 0)
        {
            Debug.LogWarning("NO Player ?");
            return;
        }

        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            Instantiate(playerPrefab, new Vector3(playerSqawnPos[i].x, 1, playerSqawnPos[i].y), playerPrefab.transform.rotation);
        }
    }

    private void GameStart()
    {
        aGameState = GameState.ReadyTimer;
        aStartCountDownTimer = START_COUNT_DOWN_TIME;
    }

    private void GameOver()
    {
        aGameState = GameState.GameOver;

    }



    private GameState aGameState;
    private float aSteamStartTimer;
    private float aSteamBreakTimer;
    private float aPressureTimer;
    private float aStartCountDownTimer;

    private List<Vector2> playerSqawnPos = new List<Vector2>();


    private float pressure = 0;
    private float pressureScale = 0.1f;

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float START_COUNT_DOWN_TIME = 5.0f;
}
