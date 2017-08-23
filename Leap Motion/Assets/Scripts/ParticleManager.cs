using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem particle;

    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
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
}
