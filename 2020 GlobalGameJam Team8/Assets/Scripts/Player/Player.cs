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

    /// 玩家鋼體
    private Rigidbody rb;

    //-----------------------------------------------------------------------
    // Public Parameter
    //-----------------------------------------------------------------------

    /// 被抓住的物件
    [HideInInspector] public GameObject holdModule;


    /// <summary>
    /// 控制器
    /// </summary>
    [HideInInspector] public IController controller;

    /// <summary>
    /// 抓住物件時物件的位置
    /// </summary>
    public GameObject holdPoint;

    /// 放置顯示物件
    public PutDisplay putDisplay;

    /// 各顏色角色物件
    public GameObject RedBody;
    public GameObject YellowBody;
    public GameObject GreenBody;
    public GameObject BlueBody;

    /// <summary>
    /// Buff狀態代理
    /// </summary>
    [HideInInspector] public BuffAgent buffAgent;

    /// <summary>
    /// 玩家當前能力值
    /// </summary>
    [HideInInspector] public PlayerDetail Detail;

    /// <summary>
    /// 玩家預設能力值
    /// </summary>
    public PlayerDetail DefaultDetail;

    //-----------------------------------------------------------------------
    // Private Function
    //-----------------------------------------------------------------------

    private void Init()
    {       

        rb = GetComponent<Rigidbody>();
        StateHandle = new IdleState(this);
        buffAgent = new BuffAgent(this);

        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            if (!PlayerManager.Instance.players[i].isSpawned)
            {
                PlayerManager.Instance.players[i].isSpawned = true;

                ///設定控制器
                if (PlayerManager.Instance.players[i].controllerType == ControllerType.Keyboard1 ||
                    PlayerManager.Instance.players[i].controllerType == ControllerType.Keyboard2)
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
        Detail = DefaultDetail;
    }
    
    private void Update()
    {
        ///Buff作用更新
        buffAgent.BuffUpdate();

        ///狀態處理更新
        StateHandle.UpdateFrame();

        ///移動能力
        Movement();

    }

    /// <summary>
    /// 移動能力
    /// </summary>
    private void Movement()
    {
        float horizontal = controller.Horizontal();
        float vertical = controller.Vertical();

        if(Mathf.Abs(rb.velocity.x) < Detail.speed && Mathf.Abs(rb.velocity.y) < Detail.speed)
        {
            rb.AddForce(new Vector3(horizontal, 0, vertical) * Detail.addSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -Detail.speed, Detail.speed), 0, Mathf.Clamp(rb.velocity.z, -Detail.speed, Detail.speed));

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > LEAST_MOVEMENT_VALUE_TO_ROTATE && Detail.speed > 0)
        {           
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0);
        }

    }

    /// <summary>
    /// 狀態改變
    /// </summary>
    /// <param name="nextState">下一個狀態</param>
    public void ChangeState(IPlayerState nextState)
    {
        StateHandle.Exit();
        StateHandle = nextState;
        StateHandle.Enter();
    }
 
    /// <summary>
    /// 設置玩家暈眩
    /// </summary>
    /// <param name="_lifeTime"></param>
    public void SetDizziness(float _lifeTime)
    {
        buffAgent.AddBuff(new Dizziness(_lifeTime));
    }


    private const float LEAST_MOVEMENT_VALUE_TO_ROTATE = 0.3f;
    public const float MAX_SPEED = 6f;

}

[System.Serializable]
public class PlayerDetail 
{
    /// 加速度
    public float addSpeed;
    /// 當前最大速度
    public float speed;
    /// 拋物件時的力道
    public float throwPower;
    /// 手長度(可取得物件以及放置物件的距離)
    public float handLength;

    public void Clear()
    {
        addSpeed = 0;
        speed = 0;
        throwPower = 0;
        handLength = 0;
    }
    public static PlayerDetail operator +(PlayerDetail a, PlayerDetail b)
    {
        PlayerDetail result=new PlayerDetail();
        result.addSpeed = a.addSpeed + b.addSpeed;
        result.speed = a.speed + b.speed;
        result.throwPower = a.throwPower + b.throwPower;
        result.handLength = a.handLength + b.handLength;
        return result;
    }

    public void Clamp(float min, float max)
    {
        addSpeed = Mathf.Clamp(addSpeed, min, max);
        speed = Mathf.Clamp(speed, min, max);
        throwPower = Mathf.Clamp(throwPower, min, max);
        handLength = Mathf.Clamp(handLength, min, max);
    }
}



