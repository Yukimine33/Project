using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource source;
    AudioClip clip;

	void Start ()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    public void EnemyDie()
    {
        AudioPlay("EnemyDie");
    }

    public void EnemySwordAudio()
    {
        AudioPlay("EnemySword");
    }

    public void EnemyBigWeaponAudio()
    {
        AudioPlay("BigSword");
    }

    public void BossStepAudio()
    {
        AudioPlay("BossStep");
    }

    public void BossAngerAudio()
    {
        AudioPlay("BossAnger");
    }

    public void BossSpecialAudio()
    {
        AudioPlay("Fire");
    }

    public void BossExplosionAudio()
    {
        AudioPlay("Explosion");
    }

    public void AudioPlay(string name)
    {
        clip = Resources.Load<AudioClip>("Audio/" + name);
        source.clip = clip;
        source.Play();
    }
}
