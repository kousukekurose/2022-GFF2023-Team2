using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPArea : MonoBehaviour
{
    public bool isEMP = false;

    // レイヤーを指定
    [SerializeField]
    private LayerMask enemyLayer = default;
    [SerializeField]
    private LayerMask playerLayer = default;
    [SerializeField]
    private LayerMask bossLayer = default;
    [SerializeField]
    private LayerMask leaderLayer = default;
    [SerializeField]
    private LayerMask itemLayer = default;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (isEMP)
        {
            Debug.Log("other:"+ other);
            var layerMask = 1 << other.gameObject.layer;
            // エネミーがトリガー内に侵入
            if ((layerMask & enemyLayer) != 0)
            {

                var enemy = other.gameObject.GetComponent<Enemy>();
            }
            if ((layerMask & playerLayer) != 0)
            {
                var player = other.transform.root.gameObject.GetComponent<MoveBehaviour>();
                player.Stan();
            }
            if ((layerMask & itemLayer) != 0)
            {
                var itemEnemy = other.transform.GetComponent<ItemEnemy>();
            }
            if ((layerMask & leaderLayer) != 0)
            {
                var leaderEnemy = other.transform.GetComponent<LeaderEnemy>();
            }
            if ((layerMask & bossLayer) != 0)
            {
                var bossEnemy = other.transform.GetComponent<BossEnemy>();
                bossEnemy.Stan();
            }
        }
    }
}
