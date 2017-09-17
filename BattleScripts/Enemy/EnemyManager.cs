using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private static EnemyManager instance;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyManager();
            }
            return instance;
        }
    }

    public List<GameObject> enemyList = new List<GameObject>(); //敌人列表
    public List<Vector3> enemyPos = new List<Vector3>(); //敌人位置列表 
}
