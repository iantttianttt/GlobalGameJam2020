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
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------
    public GameObject playerPrefab;
    public MainUI mainUI;
    public PauseUI pauseUI;
    public LevelDataObject[] LevelDatas;

    public GameState GetGameState { get { return mGameState; } }

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 設置玩家生成位置
    /// </summary>
    /// <param name="_spawnPos"></param>
    public void AddPlayerSqawnPos(Vector2 _spawnPos)
    {
        mPlayerSpawnPos.Add(_spawnPos);
    }

    /// <summary>
    /// 重設壓力進度
    /// </summary>
    public void ResetPressureTimer()
    {
        mPressureTimer = 0.0f;
        if (mGameState == GameState.FirstSteamTime)
        {
            mGameState = GameState.CoreGameTime;
        }
    }

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    private void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        InitNewGame();
    }

    private void Update()
    {
        //       mainUI.SetBar(pressure);
        switch (mGameState)
        {
            case GameState.Preparing:
                break;
            case GameState.ReadyTimer:
                mStartCountDownTimer -= Time.deltaTime;
                if (mStartCountDownTimer <= 0.0f)
                {
                    mGameState = GameState.FirstSteamTime;
                    mPressureTimer = 0.0f;
                }
                break;
            case GameState.FirstSteamTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                mPressureTimer += Time.deltaTime;
                mainUI.SetBar(mPressureTimer, mSteamStartTimer);
                if (mPressureTimer > mSteamStartTimer)
                {
                    GameOver();
                }
                break;
            case GameState.CoreGameTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                mPressureTimer += Time.deltaTime;
                mainUI.SetBar(mPressureTimer, mSteamBreakTimer);
                if (mPressureTimer > mSteamBreakTimer)
                {
                    GameOver();
                }
                break;
            case GameState.GameOver:
                break;
        }
        EveryFrameUpdate();
    }

    /// <summary>
    /// 初始化新遊戲
    /// </summary>
    private void InitNewGame()
    {
        mGameState = GameState.Preparing;
        InitLevel();
        SpawnPlayer();
        GameStart();
    }

    /// <summary>
    /// 初始化關卡
    /// </summary>
    private void InitLevel()
    {
        ModuleManager.Instance.InitModuleManager();
        ModuleManager.Instance.BuildLevel(LevelDatas[0].LevelData);
        LevelData levelData = ModuleManager.Instance.GetCurrentLevelData;
        mSteamStartTimer    = levelData.SteamStartTimer;
        mSteamBreakTimer    = levelData.SteamBreakTimer;
    }

    /// <summary>
    /// 生成玩家
    /// </summary>
    private void SpawnPlayer()
    {
        if (PlayerManager.Instance.players.Count <= 0)
        {
            Debug.LogWarning("NO Player ?");
            return;
        }

        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            Instantiate(playerPrefab, new Vector3(mPlayerSpawnPos[i].x, PLAYER_SPAWN_HIGH_ADJUST, mPlayerSpawnPos[i].y), playerPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void GameStart()
    {
        mGameState = GameState.ReadyTimer;
        mStartCountDownTimer = START_COUNT_DOWN_TIME;
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    private void GameOver()
    {
        mGameState = GameState.GameOver;
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

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private GameState     mGameState;
    private float         mSteamStartTimer;
    private float         mSteamBreakTimer;
    private float         mPressureTimer;
    private float         mStartCountDownTimer;
    private List<Vector2> mPlayerSpawnPos = new List<Vector2>();

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const float START_COUNT_DOWN_TIME    = 5.0f;
    private const float PLAYER_SPAWN_HIGH_ADJUST = -0.3f;
}
