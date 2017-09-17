using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemy : MonoBehaviour
{
    public Transform enemyParent;

    private void Start()
    {
        enemyParent = GameObject.Find("EnemyParent").GetComponent<Transform>();
        TriggerManager.Instance.triggerList.Add(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (Transform child in enemyParent)
            {
                child.GetComponent<EnemyCharacter>().WakeUp();
                child.Find("EnemyHUD").gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
}
