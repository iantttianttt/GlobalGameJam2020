using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// 玩家控制程式
/// </summary>
public class PlayerController : MonoBehaviour
{
    
    /// <summary>
    /// 控制器
    /// </summary>
    public XboxController controller;

    /// <summary>
    /// 當前狀態
    /// </summary>
    private PlayerState curentState;

    /// <summary>
    /// 移動速度
    /// </summary>
    public float speed = 5;

    /// <summary>
    /// 拋物件時的力道
    /// </summary>
    public float throwPower = 800;

    /// <summary>
    /// 手長度(可取得物件以及放置物件的距離)
    /// </summary>
    private float handLength=1f;

    /// <summary>
    /// 抓住物件時物件的位置
    /// </summary>
    public GameObject holdPoint;

    /// <summary>
    /// 被抓住的物件
    /// </summary>
    private GameObject holdModule;

    public GameObject RedBody;
    public GameObject YellowBody;
    public GameObject GreenBody;
    public GameObject BlueBody;

    /// <summary>
    /// 玩家鋼體
    /// </summary>
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curentState = PlayerState.None;

        for(int i=0;i< PlayerManager.Instance.players.Count; i++)
        {
            if (!PlayerManager.Instance.players[i].isSpawned)
            {
                PlayerManager.Instance.players[i].isSpawned = true;
                controller = PlayerManager.Instance.players[i].controller;
                break;
            }
        }

        switch (controller) {
            case XboxController.First:
                RedBody.SetActive(true);
                break;
            case XboxController.Second:
                YellowBody.SetActive(true);
                break;
            case XboxController.Third:
                GreenBody.SetActive(true);
                break;
            case XboxController.Fourth:
                BlueBody.SetActive(true);
                break;
        }

    }

    void Update()
    {
        ///移動能力
        Movement();

        ///狀態機
        switch (curentState) {
            case PlayerState.None:
                curentState = PlayerState.Idle;
                break;
            case PlayerState.Idle:
                CheckModule();
                break;
            case PlayerState.HoldingModule:
                HoldingModule();
                break;
            case PlayerState.PutingModule:
                PutDownModule();
                break;
            case PlayerState.ThrowingModule:
                ThrowModule();
                break;
        }
    }

    /// <summary>
    /// 拋物件
    /// </summary>
    public void ThrowModule()
    {
        if (holdModule)
        {
            holdModule.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            holdModule.gameObject.GetComponent<Rigidbody>().AddForce(((transform.forward + new Vector3(0, 0.7f, 0))) * throwPower);
        }
        StartCoroutine(ChangeState(0f, PlayerState.Idle));
    }

    /// <summary>
    /// 拿起物件
    /// </summary>
    public void HoldingModule()
    {
        holdModule.gameObject.transform.position = holdPoint.transform.position;

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            StartCoroutine(ChangeState(0, PlayerState.PutingModule));
        }

        if(XCI.GetButtonDown(XboxButton.B, controller))
        {
            StartCoroutine(ChangeState(0, PlayerState.ThrowingModule));
        }

    }

    /// <summary>
    /// 放下物件
    /// </summary>
    public void PutDownModule()
    {
        Vector3 putDownPos = transform.position + transform.forward* handLength;
        holdModule.gameObject.transform.position = new Vector3(Mathf.Round(putDownPos.x), 1, Mathf.Round(putDownPos.z));
        holdModule.GetComponent<Rigidbody>().isKinematic = true;
        holdModule.gameObject.layer = 9;
        StartCoroutine(ChangeState(0f, PlayerState.Idle));
    }

    /// <summary>
    /// 移動能力
    /// </summary>
    private void Movement()
    {
        float horizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float vertical = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        rb.velocity = new Vector3(horizontal, 0, vertical) * speed;

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.3f)
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0);
    }

    /// <summary>
    /// 檢查是否可以撿取物件
    /// </summary>
    private void CheckModule()
    {
        Ray ray=new Ray();
        ray.origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        ray.direction = transform.forward;
        RaycastHit raycastHit;
        if(Physics.Raycast(ray,out raycastHit, handLength, 1 << 8)){

            if (XCI.GetButtonDown(XboxButton.A,controller))
            {
                holdModule = raycastHit.transform.gameObject;
                StartCoroutine(ChangeState(0, PlayerState.HoldingModule));
            }
        }

    }

    /// <summary>
    /// 狀態切換
    /// </summary>
    /// <param name="waitTime">切換等待時間</param>
    /// <param name="nextState">下一個狀態</param>
    /// <returns></returns>
    private IEnumerator ChangeState(float waitTime,PlayerState nextState)
    {
        yield return new WaitForSeconds(waitTime);
        curentState = nextState;
    }
}
