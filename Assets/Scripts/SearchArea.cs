using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SearchArea : MonoBehaviour
{
    Score score = new Score();

    [SerializeField]
    public UnityEvent onRotationEvent;
    // 自身のTransform
    [SerializeField] private Transform _self;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 _forward = Vector3.forward;

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

    [SerializeField]
    int enemyScore = 50;
    [SerializeField]
    int leaderScore = 150;
    [SerializeField]
    int bossScore = 1000;

    [SerializeField]
    PlayerUI playerUI01 = null;
    [SerializeField]
    PlayerUI playerUI02 = null;

    [SerializeField]
    GameObject targetMarker = null;

    // 掴める範囲にあるボールをリストに格納
    public List<Enemy> Enemys { get; private set; } = new List<Enemy>();
    public List<ItemEnemy> ItemEnemys { get; private set; } = new List<ItemEnemy>();
    public List<LeaderEnemy> LeaderEnemys { get; private set; } = new List<LeaderEnemy>();

    public Item RItem { get; private set; }
    public Item LItem { get; private set; }

    Enemy near = null;
    Player player = null;
    BossEnemy bossEnemy = null;
    LeaderEnemy leaderEnemy = null;
    ItemEnemy itemEnemy = null;

    GameObject target = null;
    private float dis;
    private bool attack = false;
    private int attackPower = 1;
    private int powerCount = 0;
    [Tooltip("攻撃範囲")]
    [SerializeField]
    private float area = 3.0f;

    [SerializeField]
    ItemList itemList = null;

    public bool PlayerTarget { get; private set; } = false;

    private void Start()
    {
        targetMarker.SetActive(false);
    }

    private void Update()
    {
        if (PlayerTarget == false && player != null)
        {
            target = player.gameObject;
        }
        else if (Enemys.Count == 0 && bossEnemy == null && player == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        //エネミーターゲット
        if (Enemys.Count > 0)
        {
            TargetReader();
            target = near.gameObject;
        }
        else if (Enemys.Count == 0 && bossEnemy == null && player == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        //アイテムエネミーターゲット
        if (ItemEnemys.Count > 0 && bossEnemy == null)
        {
            ItemEnemyTargetReader();
            target = itemEnemy.gameObject;
        }
        else if (Enemys.Count == 0 && bossEnemy == null && player == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        //リーダーエネミーターゲット
        if (LeaderEnemys.Count > 0 && ItemEnemys.Count == 0 && bossEnemy == null)
        {
            LeaderEnemyTargetReader();
            target = leaderEnemy.gameObject;
        }
        else if (Enemys.Count == 0 && bossEnemy == null && player == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        //ボスエネミーターゲット
        if (bossEnemy != null && player == null)
        {
            target = bossEnemy.gameObject;
        }
        else if (Enemys.Count == 0 && bossEnemy == null && player == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        //プレイヤーターゲット
        if (player != null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0 && PlayerTarget == true)
        {
            target = player.gameObject;
        }
        else if (Enemys.Count == 0 && player == null && bossEnemy == null && LeaderEnemys.Count == 0 && ItemEnemys.Count == 0)
        {
            target = null;
        }

        if (target != null)
        {
            var tagetPos = target.transform.position;
            targetMarker.transform.position = tagetPos;
            if (bossEnemy != null && target == bossEnemy.gameObject)
            {
                targetMarker.transform.localScale = new Vector3(2f, 2f, 2f);
            }
            else
            {
                targetMarker.transform.localScale = new Vector3(0.5f, 1, 0.5f);
            }
            targetMarker.SetActive(true);

            dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis <= area)
            {
                attack = true;
            }
            else
            {
                attack = false;
            }
        }
        else
        {
            targetMarker.SetActive(false);
            attack = false;
        }
    }


    public void RotationEvent()
    {
        //範囲だったら呼び出す
        if (attack)
        {
            // ターゲットへの向きベクトル計算
            var dir = target.transform.position - _self.position;
            // ターゲットの方向への回転
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // 回転補正
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
            // 回転補正→ターゲット方向への回転の順に、自身の向きを操作する
            _self.rotation = lookAtRotation * offsetRotation;
        }
    }
    public void TargetReader()
    {
        int Count = 0;
        int number = 0;
        near = Enemys[0];
        if (near != null)
        {
            foreach (var enemy in Enemys)
            {
                if ((transform.position - enemy.transform.position).sqrMagnitude
                    < (transform.position - near.transform.position).sqrMagnitude)
                {
                    near = enemy;
                    number = Count;
                }
                Count++;
            }
            if (near.Hp <= 0)
            {
                near.Destroyme();
                score.ScorePus(transform.root.GetComponent<Player>().MultiType, enemyScore);
                Enemys.RemoveAt(number);
            }
        }
    }

    public void ItemEnemyTargetReader()
    {
        int Count = 0;
        int number = 0;
        itemEnemy = ItemEnemys[0];
        if (itemEnemy != null)
        {
            foreach (var itemEm in ItemEnemys)
            {
                if ((transform.position - itemEm.transform.position).sqrMagnitude
                    < (transform.position - itemEnemy.transform.position).sqrMagnitude)
                {
                    itemEnemy = itemEm;
                    number = Count;
                }
                Count++;
            }
            if (itemEnemy.Hp <= 0)
            {
                itemEnemy.Destroyme();
                ItemEnemys.RemoveAt(number);
            }
        }
    }

    public void LeaderEnemyTargetReader()
    {
        int Count = 0;
        int number = 0;
        leaderEnemy = LeaderEnemys[0];
        if (leaderEnemy != null)
        {
            foreach (var leaderEm in LeaderEnemys)
            {
                if ((transform.position - leaderEm.transform.position).sqrMagnitude
                    < (transform.position - leaderEnemy.transform.position).sqrMagnitude)
                {
                    leaderEnemy = leaderEm;
                    number = Count;
                }
                Count++;
            }
            if (leaderEnemy.Hp <= 0)
            {
                leaderEnemy.Destroyme();
                LeaderEnemys.RemoveAt(number);
            }
        }

    }

    //プレイヤーが他者に攻撃
    public void Damage()
    {
        if (attack)
        {
            if (near != null && target == near.gameObject)
            {
                Debug.Log("enemy");
                near.EnemyDamage(attackPower);
            }
            else if (player != null && target == player.gameObject)
            {
                Debug.Log("player");
                player.transform.GetChild(0).GetComponent<PlayerHp>().PlayerDownHp(attackPower);
            }
            else if (itemEnemy != null && target == itemEnemy.gameObject)
            {
                itemEnemy.ItemEnemyDamage(attackPower);
                if (itemEnemy.Hp <= 0)
                {
                    // アイテムをランダムに取得
                    if (RItem == null)
                    {
                        int rand = Random.Range(0, itemList.Items.Count);
                        RItem = itemList.Items[rand];
                        transform.root.GetComponent<Player>().ShowPlayerUI();
                    }
                    else if (LItem == null)
                    {
                        int rand = Random.Range(0, itemList.Items.Count);
                        LItem = itemList.Items[rand];
                        transform.root.GetComponent<Player>().ShowPlayerUI();
                    }
                    itemEnemy.Destroyme();
                }
            }
            else if (leaderEnemy != null && target == leaderEnemy.gameObject)
            {
                leaderEnemy.LeaderEnemyDamage(attackPower);
                if (leaderEnemy.Hp <= 0)
                {
                    score.ScorePus(transform.root.GetComponent<Player>().MultiType, leaderScore);
                    leaderEnemy.Destroyme();
                }
            }
            else if (bossEnemy != null && target == bossEnemy.gameObject)
            {
                Debug.Log("boss");
                bossEnemy.LoseHp(attackPower);
                if (bossEnemy.hp <= 0)
                {
                    score.ScorePus(transform.root.GetComponent<Player>().MultiType, bossScore);
                }
            }
            powerCount--;
            if (powerCount <= 0)
            {
                PowerDown();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        // エネミーがトリガー内に侵入
        if ((layerMask & enemyLayer) != 0)
        {

            var enemy = other.gameObject.GetComponent<Enemy>();
            //リストに入っていないエネミーを追加する
            if (enemy.isFree)
            {
                // リストに追加
                Enemys.Add(enemy);
                enemy.isFree = false;
            }
        }
        if ((layerMask & itemLayer) != 0)
        {
            var itemenemy = other.gameObject.GetComponent<ItemEnemy>();
            if (itemenemy.isFree)
            {
                ItemEnemys.Add(itemenemy);
                itemenemy.isFree = false;
            }
        }

        if ((layerMask & leaderLayer) != 0)
        {
            var leaderenemy = other.gameObject.GetComponent<LeaderEnemy>();
            if (leaderenemy.isFree)
            {
                LeaderEnemys.Add(leaderenemy);
                leaderenemy.isFree = false;
            }
        }

        if ((layerMask & playerLayer) != 0)
        {
            player = other.transform.root.gameObject.GetComponent<Player>();
        }

        if ((layerMask & bossLayer) != 0)
        {
            bossEnemy = other.transform.GetComponent<BossEnemy>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        // エネミーがトリガーから脱出
        if ((layerMask & enemyLayer) != 0)
        {
            // エネミーをリストから削除
            var enemy = other.gameObject.GetComponent<Enemy>();
            Enemys.Remove(enemy);
            enemy.isFree = true;
        }

        if ((layerMask & itemLayer) != 0)
        {
            var itemenemy = other.gameObject.GetComponent<ItemEnemy>();
            ItemEnemys.Remove(itemenemy);
            itemenemy.isFree = true;
        }

        if ((layerMask & leaderLayer) != 0)
        {
            var leaderenemy = other.gameObject.GetComponent<LeaderEnemy>();
            LeaderEnemys.Remove(leaderenemy);
            leaderenemy.isFree = true;
        }

        if ((layerMask & playerLayer) != 0)
        {
            player = null;
        }

        if ((layerMask & bossLayer) != 0)
        {
            bossEnemy = null;
        }
    }
    public void PowerUp(int power, int count)
    {
        attackPower = power;
        powerCount = count;
    }
    public void PowerDown()
    {
        attackPower = 1;
        transform.root.transform.GetComponent<MoveBehaviour>().StopPowerItemAnimation();
    }
    public void SetLTItem()
    {
        LItem = null;
    }
    public void SetRTItem()
    {
        RItem = null;
    }
    public void Target()
    {
        if (PlayerTarget == true)
        {
            PlayerTarget = false;
        }
        else if (PlayerTarget == false)
        {
            PlayerTarget = true;
        }
    }
}

