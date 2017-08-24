using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioListener listener; //获取listener

    public AudioClip fetchClip; //拿起杯子时的音效
    public AudioClip putDownClip; //放下杯子时的音效
    public AudioClip sodaClip; //撒苏打时的音效
    public AudioClip pourClip; //撒醋时的音效
    public AudioClip blowOutClip; //火山爆发的音效

	void Awake ()
    {
        listener = GetComponent<AudioListener>();
        fetchClip = Resources.Load<AudioClip>("Audio/拿瓶子");
        putDownClip = Resources.Load<AudioClip>("Audio/杯子放下");
        sodaClip = Resources.Load<AudioClip>("Audio/撒苏打");
        pourClip = Resources.Load<AudioClip>("Audio/洒醋");
        blowOutClip = Resources.Load<AudioClip>("Audio/火山爆发");
    }
	
	void Update ()
    {
		
	}

    /// <summary>
    /// 启用listener
    /// </summary>
    public void EnableAudioListener()
    {
        listener.enabled = true;
    }

    /// <summary>
    /// 禁用listener
    /// </summary>
    public void DisableAudioListener()
    {
        listener.enabled = false;
    }
}
