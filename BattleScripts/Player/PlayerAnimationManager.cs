using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager
{
    public string idleState; //idle状态动画
    public string moveState; //移动状态动画
    public string rollState; //翻滚状态动画
    public string attack01; //攻击1状态动画
    public string attack02; //攻击2状态动画
    public string attack03; //攻击3状态动画
    public string beHit; //被击中状态动画
    public string specialAttack; //特殊攻击状态动画
    public string switchWeapon; //切换武器状态动画
    public string death; //死亡状态动画
    public string addBlood; //有血瓶时喝血瓶状态动画
    public string emptyDrink; //没血瓶时磕血瓶的状态动画
    public string sit; //玩家坐篝火动画

    public string actionCMD = "ActionCMD"; //攻击状态参数

    private static PlayerAnimationManager instance;

    public static PlayerAnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAnimationManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 得到动画名称
    /// </summary>
    /// <param name="name"></param>
    public void GetAnimation(string name)
    {
        idleState = name + "-Idle";
        moveState = name + "-Run";
        rollState = name + "-Roll";
        attack01 = name + "-Attack01";
        attack02 = name + "-Attack02";
        attack03 = name + "-Attack03";
        beHit = name + "-GetHit";
        specialAttack = name + "-SpecialAttack";
        switchWeapon = name + "-Unsheath";
        death = name + "-Death";
        addBlood = "Player_Drinking";
        emptyDrink = "Player_EmptyDrinking";
        sit = "Player_Sitting";
    }
}
