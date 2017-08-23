using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class Demo : MonoBehaviour
{

    /// <summary>
    /// 当前触碰的手臂信息
    /// </summary>
    public string MyHand;
    /// <summary>
    /// 手指信息
    /// </summary>
    public string MyFinger;
    /// <summary>
    /// 控制器
    /// </summary>
    LeapProvider provider;

    public LeapHandController hc;

    public enum eMyHand
    {
        None,
        Left,
        Right,
    }


    public enum eState
    {
        Wait,
        Move,
    }

    public eState curState = eState.Wait;

    public eMyHand curHandState = eMyHand.None;

    public List<string> triggerList = new List<string>();


    private float curCheckTime;
    public float maxCheckTime = 0.5f;

    public Vector3 pickUpPos;
    public Vector3 pickUpEular;
    private Vector3 initPos;
    private Quaternion initRot;

    void Start()
    {
        initPos = gameObject.transform.position;
        initRot = gameObject.transform.rotation;

        //找到LeapProvider对象
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {
            CheckHand(other.gameObject.transform.parent.name);
        }

    }


    private void OnTriggerStay(Collider other)
    {
        CheckIsTriggerFlow(other.gameObject.name);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {
            CheckHand(other.gameObject.transform.parent.name,true);
        }
    }

    /// <summary>
    /// 检测手是否进入
    /// </summary>
    private void CheckHand(string rName, bool rIsRemove = false)
    {
        //检测手指
        switch (rName)
        {
            case "thumb":
                MyFinger = "大拇指";
                break;
            case "index":
                MyFinger = "食指";
                break;
            case "middle":
                MyFinger = "中指";
                break;
            case "pinky":
                MyFinger = "小指";
                break;
            case "ring":
                MyFinger = "无名指";
                break;
            case "palm":
                MyFinger = "手掌";
                break;
        }



        if (rIsRemove)
        {
            if (triggerList.Contains(MyFinger))
            {
                triggerList.Remove(MyFinger);
            }
        }
        else
        {
            if (MyFinger != "" && !triggerList.Contains(MyFinger))
            {
                triggerList.Add(MyFinger);
            }
        }
    }

    /// <summary>
    /// 检测是否抓起物体
    /// </summary>
    private void CheckIsPickUp()
    {
        if (curState == eState.Wait)
        {
            if (triggerList.Contains("中指") && triggerList.Contains("食指"))
            {
                curCheckTime += Time.deltaTime;
                if (curCheckTime > maxCheckTime)
                {
                    curCheckTime = 0;
                    curState = eState.Move;
                }
            }
        }

        if (curState == eState.Move)
        {

            if (triggerList.Count == 0)
            {
                curCheckTime += Time.deltaTime;
                if (curCheckTime > maxCheckTime)
                {
                    curCheckTime = 0;
                    curState = eState.Wait;

                    gameObject.transform.position = initPos;
                    gameObject.transform.rotation = initRot;
                }
            }
        }
    }


    /// <summary>
    /// 检测是否触发流体效果
    /// </summary>
    private void CheckIsTriggerFlow(string rName)
    {
        if (rName == "TriggerArea")
        {
            Debug.Log("进入触发区域");
            RaycastHit hitInfo;


            var orgVec = gameObject.transform.position + gameObject.transform.rotation * new Vector3(0, 0.1f, 0);
            var tarVec = gameObject.transform.position + gameObject.transform.rotation * new Vector3(0, 1, 0);

            Ray ray = new Ray(orgVec, tarVec);


            Debug.DrawLine(orgVec, tarVec, Color.red);

            if (Physics.Raycast(ray, out hitInfo,1<<8))
            {

                if (hitInfo.collider.gameObject.name == "bone2")
                {
                   
                }

                Debug.Log(hitInfo.collider.gameObject.layer);



                if (DemoManager.Intance.curState == DemoManager.eState.None && curHandState == eMyHand.Left)
                {
                    DemoManager.Intance.curState = DemoManager.eState.Ready;
                }
 
                if (DemoManager.Intance.curState == DemoManager.eState.Ready && curHandState == eMyHand.Right)
                {
                    if (hitInfo.collider.gameObject.name == DemoManager.Intance.BurningMountain.gameObject.name)
                    {
                        DemoManager.Intance.BurningMountain.enabled = true;
                    }
                }

            }
        }
    }


    /// <summary>
    /// 移动物体
    /// </summary>
    private void Move(Hand rHand)
    {
        if (curState == eState.Wait)
        {
            return;
        }

        if ((rHand.IsLeft && curHandState == eMyHand.Left) || (rHand.IsRight && curHandState == eMyHand.Right))
        {
            transform.position = rHand.PalmPosition.ToVector3() +

                       rHand.PalmNormal.ToVector3().normalized * 0.5f * 0.1f + pickUpPos;

            transform.rotation = rHand.Basis.CalculateRotation() * Quaternion.Euler(pickUpEular);
        }
    }


    void Update()
    {
        CheckIsPickUp();

        Frame frame = provider.CurrentFrame;//得到每一帧
        foreach (Hand hand in frame.Hands)//获取每一帧里面的手
        {
            Move(hand);
        }
    }
}