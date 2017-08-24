using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {


    public GameObject targetPos;
    public GameObject curPos;

    public GameObject waterEff;

    private GameObject insEff;

   // public float desTime=1;

   // float curTime;

    public bool isStart;


	void Start () {
		
	}
	
	 
	void Update ()
    {
        if (!isStart)
        {
            return;
        }

        if (insEff == null)
        {
            insEff = GameObject.Instantiate(waterEff);
            insEff.transform.SetParent(gameObject.transform);
            insEff.transform.localPosition = Vector3.zero;
            insEff.transform.localEulerAngles = Vector3.zero;
        }

        //curTime += Time.deltaTime ;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
 
        gameObject.transform.LookAt(targetPos.transform.position);
        gameObject.transform.position = curPos.transform.position;


   
       
        //if (curTime > desTime)
        //{
        //    Close();
        //}

 
    }

    public void Close()
    {
        isStart = false;
       // curTime = 0;
        if (insEff != null)
        {
            Destroy(insEff);
        }
    }
}
