using UnityEngine;
using TMPro;
using System.Collections;
using R3;
using System.Collections.Generic;
using Fusion;

public class PhotonGameManager : NetworkBehaviour
{
    private readonly List<PhotonPlayer> _spawnPlayer = new();
    [Header("HP UI 設定")]
    [SerializeField] private PlayerHpUI p1HpUI;
    [SerializeField] private PlayerHpUI p2HpUI;

    [Header("プレイヤー参加 UI 設定 (ロビー待機用)")]
    [SerializeField] private GameObject p1JoinedUI; // 1Pが参加したら表示（または緑色にする等のオブジェクト）
    [SerializeField] private GameObject p1ReadyUI;
    [SerializeField] private GameObject p2ReadyUI;
    [SerializeField] private GameObject p2JoinedUI; // 2Pが参加したら表示
    [SerializeField] private TextMeshProUGUI lobbyStatusText; // 「プレイヤーを待っています...」等のテキスト

    [Header("カウントダウン UI 設定")]
    [SerializeField] private GameObject countdownUI;

    [Header("スコア UI 設定")]
    [SerializeField] private TextMeshProUGUI p1ScoreTextUI;
    [SerializeField] private TextMeshProUGUI p2ScoreTextUI;

    private readonly ReactiveProperty<int> _p1Score = new(0);
    private readonly ReactiveProperty<int> _p2Score = new(0);

    private int _connectedPlayerCount = 0;
    private bool _isGameStarted = false;
    private readonly CompositeDisposable _disposables = new();

    private Animator _countDownAnimetor;
    private static readonly int countDownId = Animator.StringToHash("CountDown");

    private void Start()
    {
        // スコアUIの自動更新（R3）
        _p1Score.Subscribe(score => { if (p1ScoreTextUI != null) p1ScoreTextUI.text = $"1P: {score}"; }).AddTo(_disposables);
        _p2Score.Subscribe(score => { if (p2ScoreTextUI != null) p2ScoreTextUI.text = $"2P: {score}"; }).AddTo(_disposables);
        _countDownAnimetor = countdownUI.GetComponent<Animator>();

        // 【初期状態】カウントダウンは隠し、参加UIは未参加の状態にしておく
        if(p1ReadyUI != null) p1ReadyUI.SetActive(true);
        if(p2ReadyUI != null) p2ReadyUI.SetActive(true);
        if (countdownUI != null) countdownUI.SetActive(false);
        if (p1JoinedUI != null) p1JoinedUI.SetActive(false);
        if (p2JoinedUI != null) p2JoinedUI.SetActive(false);
        if (lobbyStatusText != null) lobbyStatusText.text = "対戦相手を探しています...";
    }

    /// <summary>
    /// スパウナーからプレイヤーが生成された瞬間に呼ばれるハブ関数
    /// </summary>
    public void OnPlayerSpawned(PhotonPlayer player)
    {
        _connectedPlayerCount++;
        Debug.Log($"[GameManager] プレイヤー {player.PlayerId} が合流（現在 {_connectedPlayerCount} 人）");
        _spawnPlayer.Add(player);
        player.enabled = false;
        // ⭕ 1. HP通知のリンク（R3）
        player.CurrentHp
            .Subscribe(hp =>
            {
                if(player.PlayerId == 1 && p1HpUI != null)
                {
                    Debug.Log($"プレイヤー1のHPは{hp}");
                }
                else if(player.PlayerId == 2 && p2HpUI != null)
                {
                    
                }
            }).AddTo(_disposables);

        // ⭕ 2. 【新挙動】参加したプレイヤーに応じて、それぞれの参加完了UIを表示する
        if (player.PlayerId == 1)
        {
            p1JoinedUI.SetActive(true); // 1Pの参加UIをONにする
            p1ReadyUI.SetActive(false);
        }
        else if (player.PlayerId == 2 )
        {
            p2JoinedUI.SetActive(true); // 2Pの参加UIをONにする
            p2ReadyUI.SetActive(false);
        }

        //2人（全員）揃ったら、1秒後にカウントダウンを始動！
        if (_connectedPlayerCount >= 2 && !_isGameStarted)
        {
            if (lobbyStatusText != null) lobbyStatusText.text = "対戦相手が見つかりました！";
            StartCoroutine(CountdownRoutine());
        }
    }

    /// <summary>
    /// 人が揃った後に、画面中央で秒数を数えるコルーチン
    /// </summary>
    private IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(1f);
        // ロビー待機テキストを消して、カウントダウンUIを起動
        //if (lobbyStatusText != null) lobbyStatusText.enabled = false;
        if (countdownUI != null)
        {
            countdownUI.SetActive(true);
            _countDownAnimetor.SetTrigger(countDownId);
            Debug.Log("アニメーション再生");
        }
        yield return new WaitForSeconds(4f);

        // カウントダウンが0になったらゲームスタート！
        StartGame();
    }

    private void StartGame()
    {
        _isGameStarted = true;
        
        // ロビー用の参加UIをここで綺麗に非表示にする（対戦画面をスッキリさせるため）
        if (p1JoinedUI != null) p1JoinedUI.SetActive(false);
        if (p2JoinedUI != null) p2JoinedUI.SetActive(false);
        Debug.Log("[GameManager] ★2人対戦が正式に開始されました！");
        foreach(var player in _spawnPlayer)
        {
            if(player != null)
            {
                player.enabled = true;
            }
        }
    }
    public void AddScore(int playerId, int amount)
    {
        if (playerId == 1) _p1Score.Value += amount;
        else if (playerId == 2) _p2Score.Value += amount;
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
