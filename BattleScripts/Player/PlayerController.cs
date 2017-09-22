using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter player;

    void Start ()
    {
        player = GetComponent<PlayerCharacter>();
    }
	
	void FixedUpdate ()
    {
        player.CheckState();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Debug.Log(player.isSit);
        if (!player.isDead)
        {
            if (!player.isRoll && !player.isAttack && !player.isHit && !player.isAreaAttack && !player.isSwitch && !player.isDrinking && !player.isSit)
            {
                player.Run(h, v);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !player.isRoll && !player.isPowerOver && !player.isDrinking && !player.isSit)
            {
                player.Roll();
            }

            if (Input.GetKeyDown(KeyCode.J) && !player.isPowerOver && !player.isSit)
            {
                player.Attack();
                player.isRuning = false;
            }

            if (Input.GetKeyDown(KeyCode.K) && !player.isAreaAttack && !player.isPowerOver && !player.isSit)
            {
                player.SpecialAttack();
                player.isRuning = false;
            }

            if(Input.GetKeyDown(KeyCode.Tab) && !player.isSit)
            {
                player.SwitchWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && !player.isSit)
            {
                player.AddBlood();
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                player.Sit();
            }
        }

        /*
        if (player.isAreaAttack)
        {
            float distance = 2;
            float areaAngle = 50;
            var baseVector = transform.forward * distance;

            var leftAngle = Quaternion.AngleAxis(-areaAngle, transform.up);
            var leftBorder = leftAngle * baseVector;
            Debug.DrawRay(transform.position, leftBorder, Color.green);

            var rightAngle = Quaternion.AngleAxis(areaAngle, transform.up);
            var rightBorder = rightAngle * baseVector;
            Debug.DrawRay(transform.position, rightBorder, Color.green);
        }
        */
    }
}
