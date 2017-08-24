using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoManager : MonoBehaviour
{
    public ParticleManager particle;
    public AudioSource volcanoAudioSource; //火山相关音效source
    public AudioManager audioManager;

    Animator volcanoAnima;

    bool isLeftReady = false;
    bool isRightReady = false;

    public float timerCount = 0; //记录倒东西的时间
    float timerEmission = 0; //记录火山喷发时间

	void Start ()
    {
        audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        particle = gameObject.transform.Find("Obj_Volcano/Efct_Volcano").GetComponent<ParticleManager>(); //获取火山粒子特效
        volcanoAnima = gameObject.transform.Find("Obj_Volcano/Efct_Volcano").GetComponent<Animator>(); //获取火山喷发衰减动画
        volcanoAnima.SetTrigger("Idle");
        volcanoAudioSource = GetComponent<AudioSource>();
        volcanoAudioSource.clip = audioManager.blowOutClip;
    }
	
	void Update ()
    {
        if(isLeftReady && isRightReady)
        {
            volcanoAudioSource.Play();
            particle.Play();
            volcanoAnima.SetBool("isEmission", false);
            timerEmission += Time.deltaTime;
            VolcanoEmission();
        }
    }

    /// <summary>
    /// 射线检测是否倒在了火山口区域
    /// </summary>
    /// <param name="checkTran"></param>
    public void RayCheck(Transform checkTran)
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(checkTran.position, -Vector3.up);
        Physics.Raycast(ray, out hit, 10);
        if(hit.collider.gameObject.name == "Volcano")
        {

            timerCount += Time.deltaTime;
            //Debug.Log("Hit the volcano");
            //Debug.Log(timerCount);
            if (timerCount >= 2.0f)
            {
                CheckReady(checkTran.GetComponentInParent<Cups>());
            }
        }  
    }

    /// <summary>
    /// 检查两个杯子的状态是否是已就绪
    /// </summary>
    /// <param name="checkTran"></param>
    void CheckReady(Cups checkCup)
    {
        if(checkCup.myHandState == Cups.myHand.Left)
        {
            isLeftReady = true;
        }
        else
        {
            isRightReady = true;
        }
    }

    void VolcanoEmission()
    {
        if(timerEmission > 5)
        {
            volcanoAnima.SetBool("isEmission", true);
            timerEmission = 0;
            isLeftReady = false;
            isRightReady = false;
        }

    }
}
