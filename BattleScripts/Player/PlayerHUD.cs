using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    PlayerCharacter player;

    public Slider healthSlider;
    public Slider powerSlider;

    public Text potionAmount;
    public Text fireText;
    public Text soulsText;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        healthSlider = transform.Find("Page_PlayerInfo/Sld_Health").GetComponent<Slider>();
        powerSlider = transform.Find("Page_PlayerInfo/Sld_Power").GetComponent<Slider>();

        potionAmount = transform.Find("Img_BloodPotion/Txt_Amount").GetComponent<Text>();

        fireText = transform.Find("Txt_Fire").GetComponent<Text>();
        fireText.gameObject.SetActive(false);

        soulsText = transform.Find("Txt_Souls/Text").GetComponent<Text>();
    }
	
	void Update ()
    {
        healthSlider.value = player.health;
        powerSlider.value = player.power;

        potionAmount.text = player.potionCount.ToString();
        soulsText.text = player.souls.ToString();

        if (player.isCanSit)
        {
            fireText.gameObject.SetActive(true);
            if(!player.isSit)
            {
                fireText.text = "按 \"E\" 坐篝火";
            }
            else
            {
                fireText.text = "按 \"E\" 离开";
            }
        }
        else
        {
            fireText.gameObject.SetActive(false);
        }
    }
}
