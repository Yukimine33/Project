using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFirePlace : MonoBehaviour
{
    PlayerCharacter player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.isCanSit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.isCanSit = false;
        }
    }
}
