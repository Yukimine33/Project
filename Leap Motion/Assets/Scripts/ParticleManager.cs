using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem particle;
    public AudioSource particleAudioSource; //粒子音效source
    public AudioManager audioManager;

    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
        particleAudioSource = this.GetComponent<AudioSource>();
        audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
    }

    /// <summary>
    /// 播放粒子效果
    /// </summary>
    public void Play()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 停止播放粒子效果
    /// </summary>
    public void Stop()
    {
        gameObject.SetActive(false);
    }

    public void AudioPlay()
    {
        if (gameObject.name == "Efct_Soda")
        {
            Debug.Log("Get the audio");
            particleAudioSource.PlayOneShot(audioManager.sodaClip, 0.1f);
        }
        else if (gameObject.name == "Efct_Vinegar")
        {
            particleAudioSource.PlayOneShot(audioManager.pourClip, 0.1f);
            //particleAudioSource.clip = audioManager.pourClip;
        }
        else if (gameObject.name == "Efct_Volcano")
        {
            particleAudioSource.PlayOneShot(audioManager.blowOutClip, 0.1f);
            //particleAudioSource.clip = audioManager.blowOutClip;
        }

        
    }
}
