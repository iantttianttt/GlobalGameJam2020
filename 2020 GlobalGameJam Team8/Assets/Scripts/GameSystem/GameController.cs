using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public enum GamePlayState
{
    Preparing,
    ReadyTimer,
    FirstSteamTime,
    CoreGameTime,
    GamePause,
    GameOver,
}


public class GameController : Singleton<GameController>
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------
    public MainUI mainUI;
    public PauseUI pauseUI;
    public GamePlayState GetGameState { get { return mGameState; } }

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 初始化新遊戲
    /// </summary>
    public void InitNewGame(string _LevelName)
    {
        mGameState = GamePlayState.Preparing;

        GetAllLevelDatas();
        bool InitLevelSuccess = InitLevel(_LevelName);
        if(!InitLevelSuccess) { return; }
        SpawnPlayer();
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void GameStart()
    {
        mGameState           = GamePlayState.ReadyTimer;
        mStartCountDownTimer = START_COUNT_DOWN_TIME;
    }

    public void UpdateController()
    {
        //       mainUI.SetBar(pressure);
        switch (mGameState)
        {
            case GamePlayState.Preparing:
                break;
            case GamePlayState.ReadyTimer:
                mStartCountDownTimer -= Time.deltaTime;
                if (mStartCountDownTimer <= 0.0f)
                {
                    mGameState = GamePlayState.FirstSteamTime;
                    mPressureTimer = 0.0f;
                }
                break;
            case GamePlayState.FirstSteamTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                mPressureTimer += Time.deltaTime;
//                mainUI.SetBar(mPressureTimer, mSteamStartTimer);
                if (mPressureTimer > mSteamStartTimer)
                {
                    GameOver();
                }
                break;
            case GamePlayState.CoreGameTime:
                ModuleManager.Instance.CleanModuleUpdateList();
                ModuleManager.Instance.StartCheckLinkedCount();
                ModuleManager.Instance.ModuleAutoUpdate();
                mPressureTimer += Time.deltaTime;
//                mainUI.SetBar(mPressureTimer, mSteamBreakTimer);
                if (mPressureTimer > mSteamBreakTimer)
                {
                    GameOver();
                }
                break;
            case GamePlayState.GameOver:
                break;
        }
        EveryFrameUpdate();
    }

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
        if (mGameState == GamePlayState.FirstSteamTime)
        {
            mGameState = GamePlayState.CoreGameTime;
        }
    }

    /// <summary>
    /// 清除關卡
    /// </summary>
    public void ClearGamePlayLevel()
    {
        Destroy(ModuleManager.Instance.gameObject);
        Destroy(LevelBuilder.Instance.gameObject);
        Destroy(this.gameObject);
    }

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------
    /// <summary>
    /// 取得關卡資料
    /// </summary>
    private void GetAllLevelDatas()
    {
        Object[] dataObjList = Resources.LoadAll(LEVEL_DATA_OBJECT_FLODER, typeof(LevelDataObject));
        foreach(LevelDataObject data in dataObjList)
        {
            if(!mLevelDatas.ContainsKey(data.LevelData.LevelName))
            {
                mLevelDatas.Add(data.LevelData.LevelName, data.LevelData);
            }
        }
    }

    /// <summary>
    /// 初始化關卡，回傳成功或失敗
    /// </summary>
    private bool InitLevel(string _LevelName)
    {
        ModuleManager.Instance.InitModuleManager();
        LevelBuilder.Instance.InitLevelBuilder();

        LevelData targetLevelData = null;
        if(mLevelDatas.TryGetValue(_LevelName, out targetLevelData))
        {
            ModuleManager.Instance.SetupDefaultModule(targetLevelData);
            LevelBuilder.Instance.BuildLevel(targetLevelData);
            mSteamStartTimer    = targetLevelData.SteamStartTimer;
            mSteamBreakTimer    = targetLevelData.SteamBreakTimer;
            return true;
        }
        return false;
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

        GameObject playerPrefab = Resources.Load<GameObject>(PLAYER_OBJECT_PATH);
        if(playerPrefab == null){ return; }

        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            Instantiate(playerPrefab, new Vector3(mPlayerSpawnPos[i].x, PLAYER_SPAWN_HIGH_ADJUST, mPlayerSpawnPos[i].y), playerPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    private void GameOver()
    {
        mGameState = GamePlayState.GameOver;
        Debug.Log("GameOver");
        GameManager.Instance.ChangeGameState(new State_PlayerSelect(GameManager.Instance));
//        mainUI.Lose();
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
    private GamePlayState mGameState;
    private float         mSteamStartTimer;
    private float         mSteamBreakTimer;
    private float         mPressureTimer;
    private float         mStartCountDownTimer;
    private List<Vector2> mPlayerSpawnPos = new List<Vector2>();
    private Dictionary<string, LevelData> mLevelDatas = new Dictionary<string, LevelData>( );

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    private const string LEVEL_DATA_OBJECT_FLODER = "GameSetting/LevelDataObject";
    private const string PLAYER_OBJECT_PATH       = "Prefabs/Player";
    private const float  START_COUNT_DOWN_TIME    = 5.0f;
    private const float  PLAYER_SPAWN_HIGH_ADJUST = -0.3f;

}
