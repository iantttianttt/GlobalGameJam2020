using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public enum GameState
{
    Preparing,
    ReadyTimer,
    FirstSteamTime,
    CoreGameTime,
    GamePause,
    GameOver,
}


public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;
    public MainUI mainUI;
    public PauseUI pauseUI;
    public LevelDataObject[] LevelDatas;

    public GameState GetGameState { get { return aGameState; } }

    /// <summary>
    /// 設置玩家生成位置
    /// </summary>
    /// <param name="_sqawnPos"></param>
    public void AddPlayerSqawnPos(Vector2 _sqawnPos)
    {
        playerSqawnPos.Add(_sqawnPos);
    }

    public void ResetPressureTimer()
    {
        aPressureTimer = 0.0f;
        if (aGameState == GameState.FirstSteamTime)
        {
            aGameState = GameState.CoreGameTime;
        }
    }


    void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        InitNewGame();
    }

    private void Update()
    {
        //       mainUI.SetBar(pressure);

        switch (aGameState)
        {
            case GameState.Preparing:
                break;
            case GameState.ReadyTimer:
                aStartCountDownTimer -= Time.deltaTime;
                if (aStartCountDownTimer <= 0.0f)
                {
                    aGameState = GameState.FirstSteamTime;
                    aPressureTimer = 0.0f;
                }
                break;
            case GameState.FirstSteamTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                aPressureTimer += Time.deltaTime;
                mainUI.SetBar(aPressureTimer, aSteamStartTimer);
                if (aPressureTimer > aSteamStartTimer)
                {
                    GameOver();
                }
                break;
            case GameState.CoreGameTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                aPressureTimer += Time.deltaTime;
                mainUI.SetBar(aPressureTimer, aSteamBreakTimer);
                if (aPressureTimer > aSteamBreakTimer)
                {
                    GameOver();
                }
                break;
            case GameState.GameOver:
                break;
        }

        EveryFrameUpdate();

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
        ModuleManager.Instance.InitModuleManager();
        ModuleManager.Instance.BuildLevel(LevelDatas[0].LevelData);
        LevelData levelData = ModuleManager.Instance.GetCurrentLevelData;
        aSteamStartTimer    = levelData.SteamStartTimer;
        aSteamBreakTimer    = levelData.SteamBreakTimer;
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
            Instantiate(playerPrefab, new Vector3(playerSqawnPos[i].x, 0f, playerSqawnPos[i].y), playerPrefab.transform.rotation);
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
        mainUI.Lose();
    }

    /// <summary>
    /// 每一幀更新時執行
    /// </summary>
    private void EveryFrameUpdate()
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Second) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Fourth) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Third))
        {
            pauseUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private GameState aGameState;
    private float aSteamStartTimer;
    private float aSteamBreakTimer;
    private float aPressureTimer;
    private float aStartCountDownTimer;

    private List<Vector2> playerSqawnPos = new List<Vector2>();




    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float START_COUNT_DOWN_TIME = 5.0f;
}
