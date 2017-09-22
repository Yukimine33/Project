using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerController controller;
    PlayerAudio audios;
    public Animator animator;
    AnimatorStateInfo stateInfo;

    public Transform bloodPotion;
    public int potionCount = 5;//血瓶数

    private int currentComboCount = 0; //当前连击数

    public float runSpeed; //移动速度
    public float rollSpeed; //翻滚速度

    public Transform unarmed;
    public Transform sword;
    public Transform heavySword;

    /// <summary>
    /// 攻击种类
    /// </summary>
    public enum AttackType
    {
        None,
        Normal,
        Special,
    }

    public AttackType attackType;

    public bool isRuning = false; //判断是否在移动
    public bool isRoll = false; //判断是否在翻滚
    public bool isAttack = false; //判断是否在攻击
    public bool isAreaAttack = false; //判断是否范围攻击
    bool isAttackEnemy = false; //判断角色攻击是否生效
    public bool isHit = false; //判断角色是否被攻击
    public bool isDead = false; //判断角色是否死亡
    public bool isSwitch = false; //判断角色是否在切换武器
    bool isInvincible = false; //判断角色是否无敌
    public bool isPowerOver = false; //判断体力是否消耗完毕
    bool isLimitOver = true; //判断是否过了体力槽空后的限定时间
    public bool isDrinking = false; //判断玩家是否在磕血瓶
    bool isPotionEmpty = false; //判断玩家血瓶是否为空
    public bool isSit = false; //判断玩家是否处于坐下的状态
    public bool isCanSit = false; //判断玩家是否在篝火旁且可以坐篝火

    public int health = 100; //玩家血量
    public int energy = 100; //玩家体力值
    float recoverSpeed = 0.5f; //体力恢复速度
    float recoverLimit = 1f; //体力槽空后经过这些时间后才开始恢复体力值
    float timeCount = 0; //体力槽空后开始计时
    public int souls; //玩家身上的魂量

    public Text deathText;
    bool isHide = true;

    public float power = 100; //角色体力

    int nextWeaponNum;
    GameObject nextWeapon;

    ParticleSystem specialAttackParticle;

    void Awake()
    {
        audios = GetComponent<PlayerAudio>();
        unarmed = gameObject.transform.Find(WeaponManager.Instance.WeaponPath() + "Unarmed");
        sword = gameObject.transform.Find(WeaponManager.Instance.WeaponPath() + "Sword");
        heavySword = gameObject.transform.Find(WeaponManager.Instance.WeaponPath() + "Sickle");
        bloodPotion = gameObject.transform.Find("M_ROOT/M_CENTER/BODY1/BODY2/R_CBONE/R_ARM1/R_ARM2/R_Hand/BloodPotion");
        bloodPotion.gameObject.SetActive(false);

        specialAttackParticle = gameObject.transform.Find("SpecialAttackArea/HitFX_Lightning/LightningMain").GetComponent<ParticleSystem>();

        WeaponManager.Instance.weaponList.Add(unarmed.gameObject);
        WeaponManager.Instance.weaponList.Add(sword.gameObject);
        WeaponManager.Instance.weaponList.Add(heavySword.gameObject);


        foreach(var pair in WeaponManager.Instance.weaponList)
        {
            pair.AddComponent<WeaponHit>();
            pair.SetActive(false);
        }

        PlayerAnimationManager.Instance.GetAnimation("Unarmed");
        WeaponManager.Instance.weaponList[0].SetActive(true);
    }

    void Start ()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        attackType = AttackType.None;
        deathText = GameObject.Find("Txt_Death").GetComponent<Text>();
        deathText.gameObject.SetActive(false);
    }
	
	void Update ()
    {

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if(power <= 0)
        {

            power = 0;
            isPowerOver = true;
            timeCount += Time.deltaTime;
            if(timeCount >= recoverLimit)
            {
                isLimitOver = true;
            }
            else
            {
                isLimitOver = false;
            }
        }
        else
        {
            timeCount = 0;
            isPowerOver = false;
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    public void Run(float h, float v)
    {
        var dir = GetDirection(h, v);

        Rotate(dir);

        if (Mathf.Abs(h) <= 0.5 && Mathf.Abs(v) <= 0.5)
        {
            isRuning = false;
            animator.SetBool("Run", false);
        }
        else
        {
            isRuning = true;
            animator.SetBool("Run", true);
            transform.position += transform.forward * runSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// 角色旋转（利用球面插值）
    /// </summary>
    /// <param name="dir"></param>
    void Rotate(Vector3 dir)
    {
        var targetPos = transform.position + dir;
        var playerPos = transform.position;

        Vector3 faceToDirc = targetPos - playerPos; //角色面朝目标的向量
        Quaternion faceToQuat = Quaternion.LookRotation(faceToDirc); //角色面朝目标方向的四元数

        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, 0.3f);

        transform.rotation = slerp;
    }

    /// <summary>
    /// 获取角色移动方向
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 GetDirection(float h, float v)
    {
        Vector3 dir = transform.forward;

        if (h > 0 && v == 0) //向右
        {
            dir = new Vector3(1, 0, 0);
        }
        else if (h < 0 && v == 0) //向左
        {
            dir = new Vector3(-1, 0, 0);
        }
        else if (h == 0 && v > 0) //向前
        {
            dir = new Vector3(0, 0, 1);
        }
        else if (h == 0 && v < 0) //向后
        {
            dir = new Vector3(0, 0, -1);
        }
        else if (h > 0 && v > 0) //向右前
        {
            dir = new Vector3(1, 0, 1);
        }
        else if (h > 0 && v < 0) //向右后
        {
            dir = new Vector3(1, 0, -1);
        }
        else if (h < 0 && v > 0) //向左前
        {
            dir = new Vector3(-1, 0, 1);
        }
        else if (h < 0 && v < 0) //向左后
        {
            dir = new Vector3(-1, 0, -1);
        }

        return dir.normalized;
    }

    /// <summary>
    /// 角色翻滚
    /// </summary>
    public void Roll()
    {
        animator.Play(PlayerAnimationManager.Instance.rollState, 0, 0);
    }

    public void RollCost()
    {
        power -= 20;
    }

    /// <summary>
    /// 玩家坐篝火或离开篝火
    /// </summary>
    public void Sit()
    {
        isSit = !isSit;
        animator.SetBool("Sit", isSit);

        if(isSit == true)
        {
            health = 100;
            power = 100;
            potionCount = 5;
            for(int i = 0;i < EnemyManager.Instance.enemyList.Count; i++)
            {
                EnemyManager.Instance.enemyList[i].SetActive(true);
                EnemyManager.Instance.enemyList[i].transform.position = EnemyManager.Instance.enemyPos[i];
                EnemyManager.Instance.enemyList[i].GetComponent<EnemyCharacter>().health = 100;
                EnemyManager.Instance.enemyList[i].GetComponent<EnemyCharacter>().isDead = false;
                EnemyManager.Instance.enemyList[i].GetComponent<Collider>().isTrigger = false;
            }
            for(int i = 0;i < TriggerManager.Instance.triggerList.Count; i++)
            {
                TriggerManager.Instance.triggerList[i].SetActive(true);
                TriggerManager.Instance.triggerList[i].GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    public void Attack()
    {
        if (stateInfo.IsName(PlayerAnimationManager.Instance.idleState) && currentComboCount == 0 && stateInfo.normalizedTime > 0.1f)
        {
            animator.SetInteger(PlayerAnimationManager.Instance.actionCMD, 1);
            currentComboCount = 1;
        }
        else if (stateInfo.IsName(PlayerAnimationManager.Instance.moveState) && currentComboCount == 0 && stateInfo.normalizedTime > 0.1f)
        {
            animator.SetInteger(PlayerAnimationManager.Instance.actionCMD, 4);
            currentComboCount = 1;
        }
        else if (stateInfo.IsName(PlayerAnimationManager.Instance.attack01) && currentComboCount == 1 && stateInfo.normalizedTime > 0.1f)
        {
            animator.SetInteger(PlayerAnimationManager.Instance.actionCMD, 2);
            currentComboCount = 2;
        }
        else if (stateInfo.IsName(PlayerAnimationManager.Instance.attack02) && currentComboCount == 2 && stateInfo.normalizedTime > 0.1f)
        {
            animator.SetInteger(PlayerAnimationManager.Instance.actionCMD, 3);
            currentComboCount = 3;
        }
    }

    /// <summary>
    /// 范围攻击
    /// </summary>
    public void SpecialAttack()
    {
        animator.SetTrigger("SpecialAttack");
    }

    /// <summary>
    /// 获取动画播放状态
    /// </summary>
    public void CheckState()
    {
        if (stateInfo.normalizedTime < 1)
        {
            if (stateInfo.IsName(PlayerAnimationManager.Instance.rollState))
            {
                transform.position += transform.forward * rollSpeed * Time.deltaTime;
                isRoll = true;
            }
            else if (stateInfo.IsName(PlayerAnimationManager.Instance.attack01) || stateInfo.IsName(PlayerAnimationManager.Instance.attack02) || stateInfo.IsName(PlayerAnimationManager.Instance.attack03))
            {
                isAttack = true;
                attackType = AttackType.Normal;
            }
            else if (stateInfo.IsName(PlayerAnimationManager.Instance.specialAttack))
            {
                isAreaAttack = true;
                attackType = AttackType.Special;
            }
            else if (stateInfo.IsName(PlayerAnimationManager.Instance.beHit))
            {
                isHit = true;
            }
            else if(stateInfo.IsName(PlayerAnimationManager.Instance.switchWeapon))
            {
                isSwitch = true;
            }
            else if (stateInfo.IsName(PlayerAnimationManager.Instance.addBlood) || stateInfo.IsName(PlayerAnimationManager.Instance.emptyDrink))
            {
                isDrinking = true;
            }
            else if (stateInfo.IsName(PlayerAnimationManager.Instance.moveState) || stateInfo.IsName(PlayerAnimationManager.Instance.idleState))
            {
                isRoll = false;
                isAttack = false;
                isAreaAttack = false;
                isHit = false;
                isSwitch = false;
                isDrinking = false;
                attackType = AttackType.None;
                if (power < 100 && isLimitOver)
                { power += recoverSpeed; }
            }
        }

        if((!stateInfo.IsName(PlayerAnimationManager.Instance.idleState) || !stateInfo.IsName(PlayerAnimationManager.Instance.moveState)) && stateInfo.normalizedTime > 1f)
        {
            animator.SetInteger(PlayerAnimationManager.Instance.actionCMD, 0);
            currentComboCount = 0;
            if (power < 100 && isLimitOver)
            { power += recoverSpeed; }
        }
    }

    /// <summary>
    /// 检测是否攻击到了敌人（帧事件）
    /// </summary>
    public void CheckCanAttack()
    {
        this.gameObject.GetComponentInChildren<WeaponHit>().WeaponAttack();
    }
    
    /// <summary>
    /// 被敌人攻击
    /// </summary>
    public void GetHit(int damage)
    {
        if (!isRoll)
        {
            Debug.Log("Damage" + damage);
            health -= damage;

            if (health > 0)
            {
                audios.AudioPlay("PlayerGetHit");
                animator.Play(PlayerAnimationManager.Instance.beHit, 0, 0);
            }
            else
            {
                Die();
            }
        }

        if(isDrinking)
        {
            var currentWeapon = WeaponManager.Instance.weaponList[WeaponManager.Instance.weaponNum];
            currentWeapon.SetActive(true);
            bloodPotion.gameObject.SetActive(false);
            isHide = true;
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    void Die()
    {
        animator.Play(PlayerAnimationManager.Instance.death, 0, 0);
        isDead = true;
        deathText.gameObject.SetActive(true);
        audios.AudioPlay("Die");
    }

    /// <summary>
    /// 更换武器
    /// </summary>
    public void SwitchWeapon()
    {
        animator.SetTrigger("SwitchWeapon");
        nextWeaponNum = WeaponManager.Instance.GetListNum();
        nextWeapon = WeaponManager.Instance.weaponList[nextWeaponNum];
        PlayerAnimationManager.Instance.GetAnimation(nextWeapon.name);
    }

    /// <summary>
    /// 喝血瓶
    /// </summary>
    public void AddBlood()
    {
        if (!isDrinking && potionCount > 0)
        {
            audios.AudioPlay("Drink");
            animator.SetTrigger("Drink");
            
        }
        else if(!isDrinking && potionCount == 0)
        {
            animator.SetTrigger("EmptyDrink");
        }

        //animator.Play(PlayerAnimationManager.Instance.addBlood, 0, 0);
    }

    /// <summary>
    /// 更换武器时显示或隐藏武器（帧事件）
    /// </summary>
    public void HideWeapon()
    {
        GameObject currentWeapon;

        

        if(nextWeaponNum == 0)
        {
            currentWeapon = WeaponManager.Instance.weaponList[WeaponManager.Instance.weaponList.Count - 1];
        }
        else
        {
            currentWeapon = WeaponManager.Instance.weaponList[nextWeaponNum - 1];
        }

        currentWeapon.SetActive(false);
        nextWeapon.SetActive(true);
    }

    /// <summary>
    /// 磕血瓶时控制武器及血瓶的显示与消失（帧事件）
    /// </summary>
    public void DrinkingPotion()
    {
        isHide = !isHide;
        var currentWeapon = WeaponManager.Instance.weaponList[WeaponManager.Instance.weaponNum];
        
        currentWeapon.SetActive(isHide);
        bloodPotion.gameObject.SetActive(!isHide);

        if (potionCount == 0)
        { bloodPotion.Find("Liquid").gameObject.SetActive(isHide); }
    }

    /// <summary>
    /// 加血并减少血瓶数（帧事件）
    /// </summary>
    public void ReducePotion()
    {
        if (potionCount > 0)
        {
            health += 30;
            potionCount--;
        }

        if (health >= 100)
        { health = 100; }
    }

    public void SpecialAttackParticle()
    {
        specialAttackParticle.Play();
    }
}
