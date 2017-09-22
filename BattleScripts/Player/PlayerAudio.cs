using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;

	void Start ()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    public void PlayerStepAudio()
    {
        AudioPlay("FootStep");
    }

    public void PlayerRollAudio()
    {
        AudioPlay("Roll");
    }

    public void PlayerGeiHit()
    {
        AudioPlay("PlayerGetHit");
    }

    public void PlayerPunch()
    {
        AudioPlay("Punch");
    }

    public void PlayerSwitchWeaponAudio()
    {
        AudioPlay("SwitchAudio");
    }

    public void AudioPlay(string name)
    {
        clip = Resources.Load<AudioClip>("Audio/" + name);
        source.clip = clip;
        source.Play();
    }
}
