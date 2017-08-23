using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class DemoManager : MonoBehaviour
{

    public static DemoManager Intance;
 
    public Animator BurningMountain;

    public enum eState
    {
        None,
        Ready,
        Spurt,
    }

    public eState curState = eState.None;

 
    public void Awake()
    {
        Intance = this;
    }

    public void OnCollisionEnter()
    {
        
    }
}
