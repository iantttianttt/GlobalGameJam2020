using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float speed=10;

    public GameObject holdPoint;
    public GameObject holdTube;
    public bool isHold;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizontal, 0, vertical) * speed;

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.2)
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0);

        if (isHold) HoldingTube();

        if (Input.GetKeyDown(KeyCode.X) && isHold)
        {
            PutDownTube();
            isHold = false;
        }
    }

    public void HoldingTube()
    {
        holdTube.gameObject.transform.position = holdPoint.transform.position;
    }

    public void PutDownTube()
    {
        Vector3 putDownPos = transform.position + transform.forward;
        holdTube.gameObject.transform.position = new Vector3 (Mathf.Round(putDownPos.x),1, Mathf.Round(putDownPos.z));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tube"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isHold = true;
                holdTube = collision.gameObject;
            }
        }
    }
}
