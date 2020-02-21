using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制程式
/// </summary>
public class Player : MonoBehaviour
{
    //-----------------------------------------------------------------------
    // Private Parameter
    //-----------------------------------------------------------------------
    /// 當前狀態處理
    private IPlayerState StateHandle;

    /// 被抓住的物件
    internal GameObject holdModule;

    /// 玩家鋼體
    private Rigidbody rb;

    /// 移動速度
    private float speed = 5;

    /// 拋物件時的力道
    internal float throwPower = 800;

    //-----------------------------------------------------------------------
    // Public Parameter
    //-----------------------------------------------------------------------

    /// <summary>
    /// 控制器
    /// </summary>
    public IController controller;

    /// <summary>
    /// 抓住物件時物件的位置
    /// </summary>
    public GameObject holdPoint;

    /// 手長度(可取得物件以及放置物件的距離)
    internal float handLength = 0.8f;

    public PutDisplay putDisplay;
    public GameObject RedBody;
    public GameObject YellowBody;
    public GameObject GreenBody;
    public GameObject BlueBody;

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------

    private void Init()
    {       

        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            if (!PlayerManager.Instance.players[i].isSpawned)
            {
                PlayerManager.Instance.players[i].isSpawned = true;

                ///設定控制器
                if (PlayerManager.Instance.players[i].controllerType == ControllerType.Keyboard1
                    || PlayerManager.Instance.players[i].controllerType == ControllerType.Keyboard2)
                    controller = new KeyboardController(PlayerManager.Instance.players[i].controllerType);
                else
                    controller = new JoystickController(PlayerManager.Instance.players[i].controllerType);

                ///設定模組顏色
                switch (PlayerManager.Instance.players[i].color)
                {
                    case ColorType.Red:
                        RedBody.SetActive(true);
                        break;
                    case ColorType.Blue:
                        BlueBody.SetActive(true);
                        break;
                    case ColorType.Yellow:
                        YellowBody.SetActive(true);
                        break;
                    case ColorType.Green:
                        GreenBody.SetActive(true);
                        break;
                }
                break;
            }
        }
    }

    private void Start()
    {
        Init();
        StateHandle = new IdleState(this);
    }

    private void Update()
    {
        ///移動能力
        Movement();
        StateHandle.EveryFrame();
    }

    /// <summary>
    /// 移動能力
    /// </summary>
    private void Movement()
    {
        float horizontal = controller.Horizontal();
        float vertical = controller.Vertical();
        rb.velocity = new Vector3(horizontal, 0, vertical) * speed;

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.3f)
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0);
    }

    public void ChangeState(IPlayerState nextState)
    {
        StateHandle.Exit();
        StateHandle = nextState;
        StateHandle.Enter();
    }
}



