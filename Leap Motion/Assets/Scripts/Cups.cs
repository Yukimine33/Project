using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;



public class Cups : MonoBehaviour
{
    public enum myHand
    {
        Left,
        Right,
    }

    public myHand myHandState = myHand.Left;

    public LeapProvider provider;
    public GameObject volcano; //火山
    public ParticleManager particle; //粒子控制
    public VolcanoManager volcanoManager; //火山控制

    public Transform checkTran; //瓶口用来射线检测的区域

    public AudioManager audioManager; //获取音效管理器
    public AudioSource cupAudioSource; //杯子音效source

    List<Finger> fingerList;

    Vector3 initPosition; //杯子初始位置
    Quaternion initRotation; //杯子初始旋转
    Vector3 upwardDirection;
    GameObject rigidPalm;
    public float angleCheck;
    public bool isPouring = false;

    void Start ()
    {
        audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        cupAudioSource = GetComponent<AudioSource>();

        volcanoManager = GameObject.Find("Volcano").GetComponent<VolcanoManager>();

        particle = GetComponentInChildren<ParticleManager>();
        particle.gameObject.SetActive(false);

        checkTran = gameObject.transform.Find("Tran_CheckTran");
        volcano = GameObject.Find("Volcano");
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        fingerList = new List<Finger>();

        initPosition = gameObject.transform.position;
        initRotation = gameObject.transform.rotation;
    }
	
	void Update ()
    {
        Frame frame = provider.CurrentFrame;//得到每一帧
        
        foreach (Hand hand in frame.Hands)//获取每一帧里面的手
        {
            if (hand != null)
            {
                if ((hand.IsLeft && myHandState == myHand.Left) || (hand.IsRight && myHandState == myHand.Right))
                {
                    Move(hand);
                    Pour();
                    IfBeyondBorder();
                }
            }
        }
    }



    private void Move(Hand rHand)
    {
        
        //Debug.Log("angleCheck:" + angleCheck);
        if (myHandState == myHand.Left)
        {
            rigidPalm = GameObject.Find("palmLeft");
            upwardDirection = -rigidPalm.transform.right;
        }
        else
        {
            rigidPalm = GameObject.Find("palmRight");
            upwardDirection = rigidPalm.transform.right;
        }

        var palmPosition = rHand.PalmPosition.ToVector3(); //手掌位置

        
        Debug.DrawLine(transform.position, volcano.transform.position, Color.yellow);

        fingerList = rHand.Fingers;

        
            
        var distanceToPalm = (palmPosition - transform.position).magnitude; //手掌到杯子的距离
        var distBetweenFinger = (palmPosition - fingerList[0].TipPosition.ToVector3()).magnitude; //大拇指到杯子的距离

        //Debug.Log("distanceToPalm" + distanceToPalm);
        //Debug.Log("distBetween" + distBetweenFinger);

        if (distanceToPalm < 0.15f && distBetweenFinger < 0.08f)
        {
            cupAudioSource.clip = audioManager.fetchClip;
            cupAudioSource.Play();

            transform.position = palmPosition + rHand.PalmNormal.ToVector3().normalized * 0.06f;
                
            transform.rotation = Quaternion.LookRotation(palmPosition - transform.position, upwardDirection);
        }
        else
        {
            MoveToInitPos();
        }
        
    }

    /// <summary>
    /// 判断是否倾倒液体
    /// </summary>
    public void Pour()
    {
        angleCheck = Vector3.Angle(transform.up, volcano.transform.up);

        if (angleCheck >= 90 && angleCheck <= 180)
        {
            volcanoManager.RayCheck(checkTran);
            //particle.gameObject.SetActive(true);
            particle.Play();
            particle.AudioPlay();
        }
        else
        {
            volcanoManager.timerCount = 0;
            isPouring = false;
            particle.Stop();
            //particle.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 限定边界，超出限定距离外则将瓶子归原位
    /// </summary>
    public void IfBeyondBorder()
    {
        var distanceCupToVolcano = (transform.position - volcano.transform.position).magnitude; //杯子到火山的距离
        float maxDistance = 0.4f; //限定的最大距离

        if (distanceCupToVolcano > maxDistance)
        {
            MoveToInitPos();
        }
    }

    /// <summary>
    /// 使杯子自动移回原位
    /// </summary>
    void MoveToInitPos()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, initPosition, 0.05f);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, initRotation, 0.05f);
    }
}
