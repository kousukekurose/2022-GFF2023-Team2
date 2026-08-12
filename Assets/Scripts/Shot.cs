using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    int shotDamage = 1;

    private void Start()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        // 衝突した対象がプレイヤまたはエネミーならダメージを与える
        if (collision.transform.CompareTag("PlayerObject"))
        {
            collision.transform.GetChild(0).transform.GetComponent<PlayerHp>().PlayerDownHp(shotDamage);
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Enemy>().EnemyDamage(shotDamage);
        }
        else if (collision.transform.CompareTag("ItemEnemy"))
        {
            collision.transform.GetComponent<ItemEnemy>().ItemEnemyDamage(shotDamage);
        }
        else if (collision.transform.CompareTag("ReaderEnemy"))
        {
            collision.transform.GetComponent<LeaderEnemy>().LeaderEnemyDamage(shotDamage);
        }
        else if (collision.transform.CompareTag("Boss"))
        {
            collision.transform.GetComponent<BossEnemy>().LoseHp(shotDamage);
        }
        // 自身を削除
        Destroy(gameObject);
    }
}
