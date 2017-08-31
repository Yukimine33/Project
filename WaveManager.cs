using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Transform enemyParent;
    GameObject enemy;
    GameObject[] enemyCreateArea;
    List<GameObject> enemyList;
    int amount;
    int wave;

    void Awake()
    {
        enemy = Resources.Load<GameObject>("Prefabs/Bear");
        enemyList = new List<GameObject>();
        enemyParent = GameObject.Find("EnemyParent").GetComponent<Transform>();
        enemyCreateArea = GameObject.FindGameObjectsWithTag("CreateEnemy");
    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    public void CreateEnemy()
    {
        amount = wave;
        for (int i = 0; i < enemyCreateArea.Length; i++)
        {
            for (int j = 0; j < amount - 1; j++)
            {
                var enemyClone = Instantiate(enemy, enemyCreateArea[i].transform.position, Quaternion.identity);
                enemyClone.AddComponent<BearCharacter>();
                enemyClone.transform.SetParent(enemyParent);
                enemyList.Add(enemyClone);
            }
        }
    }

    /// <summary>
    /// 检测是否已通关
    /// </summary>
    public void CheckEnemy()
    {
        if(enemyList.Count == 0)
        {

        }
    }

    public void AddWave()
    {

    }
}
