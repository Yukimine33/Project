using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    //WaveManager wave;
    public Animator enemyAnimator;
    internal AnimatorStateInfo stateInfo;

    internal string sleepState; //未苏醒状态动画
    internal string wakeState; //苏醒状态动画
    internal string idleState; //idle状态动画
    internal string moveState; //移动状态动画
    internal string hitState; //被打中状态动画
    internal string attack01; //攻击1状态动画
    internal string attack02; //攻击2状态动画
    internal string attack03; //攻击3状态动画
    internal string die; //死亡状态动画

    public PlayerCharacter player;
    public int moveSpeed = 2; //敌人移动速度

    internal bool isAttack = false; //敌人攻击状态
    internal bool isHit = false; //敌人是否被击中
    internal bool isSleeping = true; //敌人是否苏醒
    public bool isDead = false; //敌人死亡状态

    public int health; //敌人生命值

    [SerializeField]
    public int attack; //敌人攻击力

    float disappearTime; //敌人死亡到尸体消失的时间

    float alertDiatance; //敌人发现玩家的最大距离
    float minMoveDiatance; //敌人移动到据玩家该距离时停止移动
    float damageRadius; //敌人攻击判定距离

    public int souls; //敌人掉落魂量

    internal List<string> attackList = new List<string>();

    internal Transform damageArea;


    ParticleSystem deathParticle;
    ParticleSystem bloodParticle;
    Transform soulsParticle;
    Transform soulsClone;
    Transform soulsParent;

    enum Enemy
    {
        Bear,
        Skeleton_Normal,
        Skeleton_Magic,
        Skeleton_Archer,
    }

    Enemy enemyType;

    EnemyAudio enemyAudio;

	void Start ()
    {
        //wave = GameObject.Find("GameMode").GetComponent<WaveManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        GetEnemyType();
        GetAnimation(this.gameObject.tag);
        enemyAnimator = GetComponent<Animator>();

        enemyAudio = GetComponent<EnemyAudio>();

        EnemyManager.Instance.enemyList.Add(this.gameObject);
        EnemyManager.Instance.enemyPos.Add(this.gameObject.transform.position);

        deathParticle = transform.Find("Death_Particle").GetComponent<ParticleSystem>();
        bloodParticle = transform.Find("BloodParticle").GetComponent<ParticleSystem>();
        soulsParticle = Resources.Load<Transform>("Prefabs/SoulsTrailer");

        soulsClone = Instantiate(soulsParticle, this.transform.Find("SoulsBirthPlace"));
        soulsClone.gameObject.AddComponent<ParticleMove>();
        soulsClone.gameObject.SetActive(false);
        soulsParent = GameObject.Find("ParticleParent").GetComponent<Transform>();

        attackList.Add(attack01);
        attackList.Add(attack02);
        attackList.Add(attack03);
    }
	
	void Update ()
    {
        if (!isDead)
        { soulsClone.position = this.transform.Find("SoulsBirthPlace").position; }

        var playerPos = player.gameObject.transform.position;
        var distanceToPlayer = (playerPos - transform.position).magnitude; //敌人距玩家的距离

        if (player.isDead)
        {
            distanceToPlayer = 100;
        }

        stateInfo = enemyAnimator.GetCurrentAnimatorStateInfo(0);

        if (!isDead)
        {
            CheckState();
            
            if (distanceToPlayer < alertDiatance)
            {
                if (!isAttack && !isHit && !isSleeping)
                {
                    Rotate(player.gameObject.transform.position - transform.position);
                    if (distanceToPlayer >= minMoveDiatance)
                    {
                        Move();
                        enemyAnimator.SetBool("Move", true);
                    }
                }
            }
            else
            {
                enemyAnimator.SetBool("Move", false);
            }

            if (distanceToPlayer < minMoveDiatance)
            {
                //Rotate(player.gameObject.transform.position - transform.position);
                Attack(); 
            }
        }
        else
        {
            disappearTime += Time.deltaTime;
            if (disappearTime >= 3f)
            {
                soulsClone.gameObject.SetActive(true);
                soulsClone.SetParent(soulsParent);
                soulsClone.GetComponent<ParticleMove>().isMove = true;
                soulsClone.GetComponent<ParticleMove>().soulsGet = souls;
                soulsClone.GetComponentInChildren<ParticleSystem>().Play();

                disappearTime = 0;
                gameObject.SetActive(false);
                Debug.Log(EnemyManager.Instance.enemyList.Count);
            }
        }
    }

    /// <summary>
    /// 获取敌人类型
    /// </summary>
    void GetEnemyType()
    {
        if(this.gameObject.tag == "Bear")
        {
            enemyType = Enemy.Bear;
            SetEnemyInfo(100, 5, 10f, 2.5f, 1.0f, 200);
        }
        else if(this.gameObject.tag == "Skeleton_Normal")
        {
            enemyType = Enemy.Skeleton_Normal;
            SetEnemyInfo(100, 5, 10f, 2.0f, 2.0f, 100);
        }
        else if (this.gameObject.tag == "Skeleton_Magic")
        {
            enemyType = Enemy.Skeleton_Magic;
            SetEnemyInfo(100, 5, 12f, 7f, 2.0f, 100);
        }
        else if (this.gameObject.tag == "Skeleton_Archer")
        {
            enemyType = Enemy.Skeleton_Archer;
            SetEnemyInfo(100, 5, 12f, 8f, 2.0f, 100);
        }
    }

    /// <summary>
    /// 设置敌人相关数值
    /// </summary>
    /// <param name="_health"></param>
    /// <param name="_damage"></param>
    void SetEnemyInfo(int _health,int _attack, float _alertDistance, float _minMoveDiatance, float _damageRadius, int _souls)
    {
        health = _health;
        attack = _attack;
        alertDiatance = _alertDistance;
        minMoveDiatance = _minMoveDiatance;
        damageRadius = _damageRadius;
        souls = _souls;
    }

    /// <summary>
    /// 获取敌人动画
    /// </summary>
    /// <param name="name"></param>
    internal void GetAnimation(string name)
    {
        sleepState = name + "_Sleeping";
        wakeState = name + "_WakingUp";
        idleState = name + "_Idle";
        moveState = name + "_Run";
        hitState = name + "_GetHit";
        attack01 = name + "_Attack01";
        attack02 = name + "_Attack02";
        attack03 = name + "_Attack03";
        die = name + "_Die";
    }

    /// <summary>
    /// 敌人苏醒
    /// </summary>
    public void WakeUp()
    {
        if (enemyType == Enemy.Skeleton_Normal)
        { enemyAnimator.SetTrigger("WakeUp"); }
    }

    /// <summary>
    /// 敌人移动
    /// </summary>
    internal void Move()
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

    /// <summary>
    /// 敌人攻击
    /// </summary>
    public void Attack()
    {
        if (enemyType == Enemy.Bear || enemyType == Enemy.Skeleton_Normal)
        {
            int ran = Random.Range(1, 4);
            if (ran == 1)
            {
                enemyAnimator.SetInteger("Attack", 1);
                //enemyAnimator.Play(attack01, 0, 0);
            }
            else if (ran == 2)
            {
                enemyAnimator.SetInteger("Attack", 2);
                //enemyAnimator.Play(attack02, 0, 0);
            }
            else if (ran == 3)
            {
                enemyAnimator.SetInteger("Attack", 3);
                //enemyAnimator.Play(attack03, 0, 0);
            }
        }
        else if (enemyType == Enemy.Skeleton_Magic)
        {
            
        }
        else if (enemyType == Enemy.Skeleton_Archer)
        {

        }
    }

    public void GetHit(int damage)
    {
        health -= damage;
        enemyAudio.AudioPlay("Hit");
        bloodParticle.Play();

        if (!isDead)
        {
            enemyAnimator.Play(hitState, 0, 0);
            
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("Die");
        isDead = true;
        deathParticle.Play();
        transform.GetComponent<Collider>().isTrigger = true;
        //transform.GetComponent<Rigidbody>().isKinematic = true;
        //wave.waveAmount--;
    }

    public void CheckState()
    {
        if (stateInfo.normalizedTime < 1)
        {
            if (stateInfo.IsName(attack01) || stateInfo.IsName(attack02) || stateInfo.IsName(attack03))
            {
                isAttack = true;
                gameObject.GetComponent<Rigidbody>().mass = 100;
                gameObject.GetComponent<Rigidbody>().drag = 100;
            }
            else if (stateInfo.IsName(hitState))
            {
                isHit = true;
            }
            else if (stateInfo.IsName(sleepState) || stateInfo.IsName(wakeState))
            {
                isSleeping = true;
            }
            else if (stateInfo.IsName(moveState) || stateInfo.IsName(idleState) || stateInfo.IsName(wakeState))
            {
                enemyAnimator.SetInteger("Attack", 0);
                isAttack = false;
                isHit = false;
                isSleeping = false;
                gameObject.GetComponent<Rigidbody>().mass = 10;
                gameObject.GetComponent<Rigidbody>().drag = 10;
            }
        }

        /*
        if (stateInfo.normalizedTime > 1f)
        {
            isAttack = false;
            enemyAnimator.SetInteger("Attack", 0); 
        }
        */
        
    }

    /// <summary>
    /// 获取生成相交球的原点
    /// </summary>
    /// <returns></returns>
    Vector3 GetCheckPos()
    {
        if (enemyType == Enemy.Bear)
        {
            var damageArea = gameObject.transform.Find("Hammer/Sphere"); //获取武器上的判定点
            return damageArea.position;
        }
        else if (enemyType == Enemy.Skeleton_Normal || enemyType == Enemy.Skeleton_Magic || enemyType == Enemy.Skeleton_Archer)
        {
            return gameObject.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void CheckCanAttack()
    {
        var checkPos = GetCheckPos();
        var damageOverLarp = Physics.OverlapSphere(checkPos, damageRadius);


        if (damageOverLarp.Length > 0)
        {
            foreach (var pair in damageOverLarp)
            {
                if (pair.gameObject.tag == "Player")
                {
                    if (enemyType == Enemy.Bear)
                    {
                        Debug.Log("Hit 1");
                        player.GetHit(attack);
                    }
                    else if (enemyType == Enemy.Skeleton_Normal || enemyType == Enemy.Skeleton_Magic || enemyType == Enemy.Skeleton_Archer)
                    {
                        var target = pair.gameObject.transform.position - transform.position;
                        var angle = Vector3.Angle(transform.forward, target);

                        if (angle <= 30 && !pair.gameObject.GetComponent<PlayerCharacter>().isDead)
                        {
                            Debug.Log("Hit 2");
                            player.GetHit(attack);
                        }
                    }
                }
            }
        }
    }
}
