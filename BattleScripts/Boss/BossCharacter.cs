using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : MonoBehaviour
{
    Animator bossAnimator;
    AnimatorStateInfo stateInfo;
    PlayerCharacter player;
    
    private string idleState; //idle状态动画
    private string moveState; //移动状态动画
    private string hitState; //被打中状态动画
    private string attack01; //攻击1状态动画
    private string attack02; //攻击2状态动画
    private string attack03; //攻击3状态动画
    private string die; //死亡状态动画
    private string walk; //走路状态动画
    private string attack04; //攻击4状态动画
    private string attack05; //攻击5状态动画
    private string kick; //踢状态动画
    private string specialAttack01; //特殊攻击状态动画
    private string specialAttack02; //特殊攻击状态动画

    bool isSecondPart = false;
    bool isAttack = false; //敌人攻击状态
    bool isHit = false; //敌人是否被击中
    public bool isDead = false; //敌人死亡状态

    int normalDamage = 20; //普通攻击伤害
    int specialDamage = 30; //特殊攻击伤害
    public int health = 300; //血量
    int moveSpeed = 2; //移动速度

    ParticleSystem specialParticle01;
    ParticleSystem specialParticle02;
    ParticleSystem specialParticle03;
    ParticleSystem deathParticle;
    ParticleSystem bloodParticle;

    GameObject[] fireParticle;

    float disappearTime;

    enum AttackType
    {
        WeaponAttack,
        KickAttack,
        SpecialAttack01,
        SpecialAttack02,
    }

    AttackType attackType;

    void Start ()
    {
        specialParticle01 = transform.Find("SpecialAttack01/Fire_GroundExplosion").GetComponent<ParticleSystem>();
        specialParticle02 = transform.Find("Armature/Hip/Bone.002/Chest/Shoulder.l/Upperarm.l/LowerArm.l/Hand.l/SpecialAttack02/FireMain").GetComponent<ParticleSystem>();
        specialParticle03 = transform.Find("SpecialAttack03/ShockWave").GetComponent<ParticleSystem>();
        deathParticle = transform.Find("DeathParticle/SkullHead").GetComponent<ParticleSystem>();
        bloodParticle = transform.Find("BloodParticle").GetComponent<ParticleSystem>();

        fireParticle = GameObject.FindGameObjectsWithTag("BossFire");
        foreach(var pair in fireParticle)
        {
            pair.SetActive(false);
        }

        bossAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        specialParticle01.gameObject.SetActive(false);
        specialParticle02.gameObject.SetActive(false);
        GetExtraAnimation();
    }
	
	void Update ()
    {
        var playerPos = player.gameObject.transform.position;
        var distanceToPlayer = (playerPos - transform.position).magnitude; //敌人距玩家的距离

        if (player.isDead)
        {
            distanceToPlayer = 100;
        }

        stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);

        if (!isDead)
        {
            CheckBossState();

            if (distanceToPlayer < 15)
            {
                if (!isAttack && !isHit)
                {
                    Rotate(player.gameObject.transform.position - transform.position);
                    if (distanceToPlayer >= 3)
                    {
                        BossMove();
                        bossAnimator.SetBool("Walk", true);
                    }
                }
            }
            else
            {
                bossAnimator.SetBool("Walk", false);
            }

            if (distanceToPlayer < 3 && !isAttack)
            {
                if (!isSecondPart)
                { //Rotate(player.gameObject.transform.position - transform.position);
                    BossAttack();
                }
                else
                {
                    SecondPartAttack();
                }
            }
            
        }
        else
        {
            disappearTime += Time.deltaTime;

            if(disappearTime >= 6f)
            {
                deathParticle.Play();
            }

            if (disappearTime >= 10f)
            {
                player.souls += 10000;
                disappearTime = 0;
                gameObject.SetActive(false);
                player.deathText.gameObject.SetActive(false);
            }
        }
    }

    private void BossMove()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 敌人旋转（利用球面插值）
    /// </summary>
    /// <param name="dir"></param>
    internal void Rotate(Vector3 dir)
    {
        var targetPos = transform.position + dir;
        var skeletomPos = transform.position;

        Vector3 faceToDirc = targetPos - skeletomPos; //角色面朝目标的向量
        Quaternion faceToQuat = Quaternion.LookRotation(faceToDirc); //角色面朝目标方向的四元数

        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, 0.2f);

        transform.rotation = slerp;
    }

    private void CheckBossState()
    {
        if (stateInfo.normalizedTime < 1)
        {
            if (stateInfo.IsName(attack01) || stateInfo.IsName(attack02) || stateInfo.IsName(attack03) || stateInfo.IsName(attack04) || stateInfo.IsName(attack05))
            {
                attackType = AttackType.WeaponAttack;
                isAttack = true;
            }
            else if (stateInfo.IsName(specialAttack01))
            {
                attackType = AttackType.SpecialAttack01;
                isAttack = true;
            }
            else if (stateInfo.IsName(specialAttack02))
            {
                attackType = AttackType.SpecialAttack02;
                isAttack = true;
            }
            else if(stateInfo.IsName(kick))
            {
                attackType = AttackType.KickAttack;
                isAttack = true;
            }
            else if (stateInfo.IsName(hitState))
            {
                isHit = true;
            }
            else if (stateInfo.IsName(idleState) || stateInfo.IsName(walk))
            {
                bossAnimator.SetInteger("Combo", 0);
                isAttack = false;
                isHit = false;
            }
        }

        if (stateInfo.normalizedTime > 1)
        {
            bossAnimator.SetInteger("Combo", 0);
        }
    }

    private void GetExtraAnimation()
    {
        idleState = "Boss_Idle";
        hitState = "Boss_GetHit";
        attack01 = "Boss_Attack01";
        attack02 = "Boss_Attack02";
        attack03 = "Boss_Attack03";
        die = "Boss_Die";
        walk = "Boss_Walk";
        attack04 = "Boss_Attack04";
        attack05 = "Boss_Attack05";
        kick = "Boss_Attack-Kick";
        specialAttack01 = "Boss_SpecialAttack01";
        specialAttack02 = "Boss_SpecialAttack02";
    }

    private void BossAttack()
    {
        int ran = Random.Range(1, 4);
        if (ran == 1)
        {
            bossAnimator.SetInteger("Combo", 1);
        }
        else if (ran == 2)
        {
            bossAnimator.SetInteger("Combo", 2);
        }
        else if (ran == 3)
        {
            bossAnimator.SetInteger("Combo", 3);
        }
    }

    private void SecondPartAttack()
    {
        int ran = Random.Range(1, 5);
        Debug.Log("ran:" + ran);
        if (ran == 1)
        {
            bossAnimator.SetInteger("Combo", 4);
        }
        else if (ran == 2)
        {
            bossAnimator.SetInteger("Combo", 5);
        }
        else if (ran == 3)
        {
            bossAnimator.SetInteger("Combo", 6);
        }
        else if (ran == 4)
        {
            bossAnimator.SetInteger("Combo", 7);
        }
    }

    /// <summary>
    /// 获取生成相交球的原点
    /// </summary>
    /// <returns></returns>
    Vector3 GetCheckPos()
    {
        if (attackType == AttackType.SpecialAttack01)
        {
            var damageArea = gameObject.transform.Find("SpecialAttack01"); //获取武器上的判定点
            return damageArea.position;
        }
        else if(attackType == AttackType.SpecialAttack02)
        {
            var damageArea = gameObject.transform.Find("Armature/Hip/Bone.002/Chest/Shoulder.l/Upperarm.l/LowerArm.l/Hand.l/SpecialAttack02"); //获取武器上的判定点
            return damageArea.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Boss普通攻击
    /// </summary>
    public void BossNormalAttack()
    {
        var damageOverLarp = Physics.OverlapSphere(gameObject.transform.position, 3);
        
        if (damageOverLarp.Length > 0)
        {
            foreach (var pair in damageOverLarp)
            {
                if (pair.gameObject.tag == "Player")
                {
                    var target = pair.gameObject.transform.position - transform.position;
                    var angle = Vector3.Angle(transform.forward, target);

                    if (angle <= 50 && !pair.gameObject.GetComponent<PlayerCharacter>().isDead)
                    {
                        player.GetHit(normalDamage);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Boss特殊攻击
    /// </summary>
    public void BossSpecialAttack()
    {
        var damagePos = GetCheckPos();
        var damageOverLarp = Physics.OverlapSphere(damagePos, 5);

        if (attackType == AttackType.SpecialAttack01)
        {
            specialParticle01.gameObject.SetActive(true);
            specialParticle01.Play();
        }
        else if (attackType == AttackType.SpecialAttack02)
        {
            specialParticle02.gameObject.SetActive(true);
            specialParticle02.Play();
        }
        

        Debug.Log("SpecialAttack");
        if (damageOverLarp.Length > 0)
        {
            foreach (var pair in damageOverLarp)
            {
                if (pair.gameObject.tag == "Player")
                {
                    if (attackType == AttackType.SpecialAttack01)
                    {
                        player.GetHit(specialDamage);
                    }
                    else if (attackType == AttackType.SpecialAttack02)
                    {
                        var target = pair.gameObject.transform.position - damagePos;
                        var angle = Vector3.Angle(transform.forward, target);

                        if (angle <= 120 && target.magnitude <= 3f && !pair.gameObject.GetComponent<PlayerCharacter>().isDead)
                        {
                            player.GetHit(specialDamage);
                        }
                    }
                    
                }
            }
        }
    }

    public void BossGetHit(int damage)
    {
        health -= damage;
        /*
        if (!isDead)
        { enemyAnimator.Play(hitState, 0, 0); }
        */

        this.gameObject.GetComponent<EnemyAudio>().AudioPlay("Hit");
        bloodParticle.Play();

        if(health <= 150 && !isSecondPart)
        {
            bossAnimator.Play("Boss_GetHit", 0,0);

            isSecondPart = true;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
       
        bossAnimator.SetTrigger("Die");
        GameObject.Find("Main Camera").GetComponent<AudioManager>().StopBGM();
        
        isDead = true;
        

        player.deathText.gameObject.SetActive(true);
        player.deathText.text = "You Defeated";

        transform.GetComponent<Collider>().isTrigger = true;
        transform.GetComponent<Rigidbody>().isKinematic = true;

        foreach (var pair in fireParticle)
        {
            pair.SetActive(false);
        }
        isSecondPart = false;
    }

    public void HideParticle()
    {
        specialParticle01.gameObject.SetActive(false);
        specialParticle02.gameObject.SetActive(false);
    }

    public void ActiveFire()
    {
        foreach (var pair in fireParticle)
        {
            pair.SetActive(true);
        }

        specialParticle03.Play();
    }

    /// <summary>
    /// 跺脚时伤害（帧事件）
    /// </summary>
    public void AngerDamage()
    {
        var damageArea = gameObject.transform.Find("SpecialAttack03");
        var damageOverLarp = Physics.OverlapSphere(damageArea.position, 4);
        if (damageOverLarp.Length > 0)
        {
            foreach (var pair in damageOverLarp)
            {
                if (pair.gameObject.tag == "Player")
                {
                    player.GetHit(specialDamage);
                }
            }
        }
    }

    public void ChangeSpeed()
    {
        bossAnimator.speed = 0.5f;
    }

    public void RecoverSpeed()
    {
        bossAnimator.speed = 1f;
    }
}
