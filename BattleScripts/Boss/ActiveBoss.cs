using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveBoss : MonoBehaviour
{
    [SerializeField]
    GameObject boss;
    [SerializeField]
    Slider bossHUD;

    AudioManager audioManager;
    PlayerCharacter player;

	void Start ()
    {
        TriggerManager.Instance.triggerList.Add(this.gameObject);
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossHUD = boss.transform.Find("Page_Boss/Sld_BossHealth").GetComponent<Slider>();
        audioManager = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if(boss.GetComponent<BossCharacter>().isDead)
        {
            this.gameObject.GetComponent<Collider>().isTrigger = true;
        }

        if(player.isSit)
        {
            audioManager.StopBGM();
            bossHUD.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !boss.GetComponent<BossCharacter>().isDead)
        {
            audioManager.ChangeBGM();
            bossHUD.gameObject.SetActive(true);
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (!boss.GetComponent<BossCharacter>().isDead)
        {
            this.gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }
    */
}
