using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossEnemy : MonoBehaviour
{
    // 体力
    [SerializeField]
    int bossHp = 20;
    public int hp { private set; get; }

    [SerializeField]
    PlayerCaster playerCaster = null;

    // 攻撃可能になるまでの時間
    [SerializeField]
    float nextAttackTime = 8;

    // 前回攻撃からの経過時間
    [SerializeField]
    float nextAttackCount = 0;

    // ランダム攻撃ダメージ
    [SerializeField]
    int randomDamage = 3;
    // 範囲攻撃ダメージ
    [SerializeField]
    int areaDamage = 3;

    //サンド管理
    AudioSource audioSource;
    [SerializeField]
    private AudioClip comeSE = null;
    [SerializeField]
    private AudioClip dieSE = null;
    [SerializeField]
    private AudioClip chargeSE = null;   
    [SerializeField]
    private AudioClip attackSE = null;
    [SerializeField]
    private AudioClip skillSE = null;

    static readonly int areaAttackId = Animator.StringToHash("AreaAttack");
    static readonly int randomAttackId = Animator.StringToHash("RandomAttack");
    static readonly int deadId = Animator.StringToHash("Dead");
    Animator animator;

    GameObject[] Players;

    public TextMeshProUGUI bossHpText;

    [SerializeField] private GameObject bossImage;
    [SerializeField] private GameObject bossdamage;

    // ボスの状態を管理
    enum MotionState
    {
        Summon,
        Idle,
        AreaAttack,
        RandomAttack,
        Dead,
        Stan,
    }
    MotionState currentState = MotionState.Summon;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        Players = GameObject.FindGameObjectsWithTag("Player");
        hp = bossHp;
        bossHpText.enabled = true;
        bossImage.SetActive(true);
        bossdamage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MotionState.Summon:
                break;
            case MotionState.Idle:
                nextAttackCount += Time.deltaTime;
                // 範囲内にプレイヤがいるか
                if (playerCaster.Players.Count != 0)
                {
                    if (nextAttackCount > nextAttackTime)
                    {
                        // いれば範囲攻撃
                        AreaAttack(playerCaster.Players);
                    }
                }
                break;
            case MotionState.AreaAttack:
                break;
            case MotionState.RandomAttack:
                break;
            case MotionState.Dead:
                break;
            case MotionState.Stan:
                break;
            default:
                break;
        }
    }
    // 範囲攻撃
    private void AreaAttack(List<Player> Players)
    {
        // ステータス更新
        currentState = MotionState.AreaAttack;
        StartCoroutine(OnAreaAttack(Players));
        audioSource.clip = attackSE;
        audioSource.Play();
    }
    IEnumerator OnAreaAttack(List<Player> Players)
    {
        bossImage.SetActive(true);
        bossdamage.SetActive(false);
        animator.SetTrigger(areaAttackId);
        // 範囲にいるプレイヤにダメージ
        yield return new WaitForSeconds(3.5f);
        if (Players.Count != 0)
        {
            foreach (var player in Players)
            {
                player.transform.GetChild(0).GetComponent<PlayerHp>().PlayerDownHp(areaDamage);
            }
        }
        nextAttackCount = 0;
    }
    private void SetStateToIdle()
    {
        currentState = MotionState.Idle;
    }
    // ランダム攻撃
    private void RandomAttack()
    {
        // ステータス更新
        currentState = MotionState.RandomAttack;
        StartCoroutine(OnRandomAttack());
        audioSource.clip = skillSE;
        audioSource.Play();

    }
    IEnumerator OnRandomAttack()
    {
        int random = Random.Range(0, 2);
        // 攻撃対象を向く
        transform.localRotation = Quaternion.LookRotation(Players[random].transform.position - transform.position);
        animator.SetTrigger(randomAttackId);
        // ランダムなプレイヤにダメージ
        yield return new WaitForSeconds(1.5f);
        Players[random].GetComponent<PlayerHp>().PlayerDownHp(randomDamage);
        nextAttackCount = 0;
    }
    public void LoseHp(int damage)
    {
        // HPを減らす
        hp -= damage;
        bossImage.SetActive(false);
        bossdamage.SetActive(true);
        // テキスト表示
        bossHpText.text = string.Format("{0}", hp);
        // 0になったら自信を削除
        if (hp <= 0)
        {
            currentState = MotionState.Dead;
            StartCoroutine(BossEnemyDestroy());
            bossHpText.enabled = false;
            bossImage.SetActive(false);
            bossdamage.SetActive(false);
            audioSource.clip = dieSE;
            audioSource.Play();
        }
    }

    IEnumerator BossEnemyDestroy()
    {
        animator.SetTrigger(deadId);
        transform.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
    public void Stan()
    {
        StartCoroutine(OnStan());
    }
    IEnumerator OnStan()
    {
        currentState = MotionState.Stan;
        animator.speed = 0;
        Debug.Log("AS:"+animator.speed);
        yield return new WaitForSeconds(2);
        animator.speed = 1;
        currentState = MotionState.Idle;
    }
}
