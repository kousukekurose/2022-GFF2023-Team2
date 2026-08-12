using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//オブジェクトにNavMeshAgentコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class ItemEnemy : MonoBehaviour
{
    //エネミーHP
    public int Hp = 3;
    public bool isFree = true;

    Vector3 playerPos;
    float distance;
    //赤いワイヤー
    [SerializeField]
    float trackingRange = 5f;


    bool tracking = false;
    NavMeshAgent agent;

    // キャラクターアニメーションを管理しているAnimatorを指定します。
    [SerializeField]
    private Animator animator = null;
    
    //サンド管理
    AudioSource audioSource;
    [SerializeField]
    private AudioClip dieSE = null;
    [SerializeField]
    private AudioClip enemyAttackSE = null;

    // AnimatorのパラメーターID
    static readonly int walkId = Animator.StringToHash("Walk");
    static readonly int IdleId = Animator.StringToHash("Idle");
    static readonly int DieId = Animator.StringToHash("Die");

    public List<GameObject> Players { get; private set; } = new List<GameObject>();

    //最大Hpの画像を入れる箱
    public Image[] hearts;
    //HPの画像を入れるもの
    public Sprite fullHeart;
    //HP差し替え用画像を入れるもの
    public Sprite emptyHeart;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Players.AddRange(GameObject.FindGameObjectsWithTag("PlayerObject"));
        agent = GetComponent<NavMeshAgent>();
        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //最初にプレイヤーのポジションを獲得する。
        IsTraking(Players[0]);
        IsTraking(Players[1]);
        var count = Players.Count;
        if (count == 0)
        {
            // エネミーが出現するときにはプレイヤーは２人いるので通常このルートは通らない
        }
        else if (count == 1 && Players[0].GetComponent<Player>().isTraking)
        {
            // エネミーが出現するときにはプレイヤーは２人いるので通常このルートは通らない

        }
        // 人数が２の場合
        else if (count == 2)
        {
            // プレイヤー１と２が追尾可能の場合
            if (Players[0].GetComponent<Player>().isTraking && Players[1].GetComponent<Player>().isTraking)
            {
                TargetReader();
                MultiTracking();
                animator.SetTrigger(walkId);
            }
            //マルチ追尾処理
            // プレイヤー１のみ追尾可能な場合
            else if (Players[0].GetComponent<Player>().isTraking && !Players[1].GetComponent<Player>().isTraking)
            {
                animator.SetTrigger(walkId);
                SingleTracking(Players[0]);
            }
            // シングル追尾処理
            // プレイヤー２のみ追尾可能な場合
            else if (!Players[0].GetComponent<Player>().isTraking && Players[1].GetComponent<Player>().isTraking)
            {
                animator.SetTrigger(walkId);
                SingleTracking(Players[1]);
            }
            // シングル追尾処理
            // プレイヤー１と２が追尾不可能な場合
            else if (!Players[0].GetComponent<Player>().isTraking && !Players[1].GetComponent<Player>().isTraking)
            {
                // 待機処理
                //追跡の時、
                agent.destination = transform.position;
                animator.SetTrigger(IdleId);

            }
            //HPの画像差し替えするためのコード
            foreach (Image img in hearts)
            {
                img.sprite = emptyHeart;
            }
            for (int i = 0; i < Hp; i++)
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }

    #region プレイヤーから攻撃を受けた時の処理
    //エネミーが攻撃を受ける
    public void ItemEnemyDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Destroyme();
            Destroyme();
            audioSource.clip = dieSE;
            audioSource.Play();
        }
    }

    //エネミーのHPが０になったらエネミーを消す
    public void Destroyme()
    {
        StartCoroutine(ItemEnemyDie());
    }
    IEnumerator ItemEnemyDie()
    {
        Hp = 0;
        animator.SetTrigger(DieId);
        isFree = false;
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
    #endregion
    #region プレイヤーの座標確認(追尾機能)
    //一人のプレイヤーのポジションを獲得
    private void SingleTracking(GameObject player)
    {
        //Playerを目標とする
        agent.destination = player.GetComponent<Player>().transform.position;
    }
    //マルチ用プレイヤーポジション
    private void MultiTracking()
    {
        //Playerを目標とする
        agent.destination = playerPos;
    }
    //プレイヤー１とプレイヤー２がどちらが近いか遠いか計算。
    public void TargetReader()
    {
        //プレイヤー１と２の距離を計算
        if ((transform.position - Players[0].transform.position).sqrMagnitude
            < (transform.position - Players[1].transform.position).sqrMagnitude)
        {
            //プレイヤー１の距離
            playerPos = Players[0].transform.position;
        }
        else
        {
            //プレイヤー２の距離
            playerPos = Players[1].transform.position;
        }
    }
    //プレイヤーがエネミーの範囲に入ったら追尾するか追尾しないかを判断
    public void IsTraking(GameObject player)
    {
        distance = Vector3.Distance(player.GetComponent<Player>().transform.position, this.transform.position);
        //PlayerがtrackingRangeより近づいたら追跡開始
        if (tracking)
        {
            //追跡の時、quitRangeより距離が離れたら中止
            if (distance > trackingRange)
            {
                tracking = false;
                animator.SetTrigger(IdleId);
                //PlayerスプリクトにisTracking
                player.GetComponent<Player>().isTraking = false;
            }
        }
        else
        {
            //PlayerがtrackingRangeより近づいたら追跡開始
            if (distance < trackingRange)
            {
                animator.SetTrigger(walkId);
                tracking = true;
                //PlayerスプリクトにisTracking
                player.GetComponent<Player>().isTraking = true;
            }
            else if (distance > trackingRange)
            {
                animator.SetTrigger(IdleId);
                //PlayerスプリクトにisTracking
                player.GetComponent<Player>().isTraking = false;
            }
        }
    }

    //エネミーの追尾範囲をシーンに可視化
    void OnDrawGizmosSelected()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
    #endregion
}
