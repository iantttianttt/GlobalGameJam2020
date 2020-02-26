using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : Singleton<SceneController>
{
    //-----------------------------------------------------------------------
    //Public Parameter
    //-----------------------------------------------------------------------

    //-----------------------------------------------------------------------
    // Get
    //-----------------------------------------------------------------------
    public bool SceneLoadingHold { get { return mSceneLoadingHold; } }

    //-----------------------------------------------------------------------
    // Public Function
    //-----------------------------------------------------------------------
    public AsyncOperation LoadScene (string _SceneName, bool _UseLoadingScene = false, bool _ClearAllScene = true)
    {
        AsyncOperation asyncLoad = null;
        if(_ClearAllScene)
        {
            if(_UseLoadingScene)
            {
                SceneManager.LoadScene(SCENE_NAME_LOAD_SCENE,LoadSceneMode.Single);
                asyncLoad = SceneManager.LoadSceneAsync(_SceneName, LoadSceneMode.Single);
            }
            else
            {
                asyncLoad = SceneManager.LoadSceneAsync(_SceneName, LoadSceneMode.Single);
            }
        }
        else
        {
            if(_UseLoadingScene)
            {
                SceneManager.LoadScene(SCENE_NAME_LOAD_SCENE,LoadSceneMode.Additive);
                asyncLoad = SceneManager.LoadSceneAsync(_SceneName, LoadSceneMode.Additive);
            }
            else
            {
                asyncLoad = SceneManager.LoadSceneAsync(_SceneName, LoadSceneMode.Additive);
            }
        }
        return asyncLoad;
    }

    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    private bool mSceneLoadingHold = false;

    //-----------------------------------------------------------------------
    // Const
    //-----------------------------------------------------------------------
    public const string SCENE_NAME_LOAD_SCENE    = "LoadingScene";
    public const string SCENE_NAME_START_MENU    = "StartMenu";
    public const string SCENE_NAME_PLAYER_SELECT = "PlayerSelect";
    public const string SCENE_NAME_LEVEL_SELECT  = "LevelSelect";
    public const string SCENE_NAME_GAME_SCENE    = "GameScene";


}
