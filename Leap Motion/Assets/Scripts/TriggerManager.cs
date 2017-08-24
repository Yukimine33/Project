using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public Cups cupLeft;
    public Cups cupRight;
    public AudioManager audioManager;
    public AudioSource triggerAudioSource;

	void Start ()
    {
        cupLeft = GameObject.Find("Cup_Left").GetComponent<Cups>();
        cupRight = GameObject.Find("Cup_Right").GetComponent<Cups>();
        audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        triggerAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cup")
        {
            triggerAudioSource.PlayOneShot(audioManager.putDownClip);
        }
    }
}
