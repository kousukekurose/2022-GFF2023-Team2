using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using R3;

// ⭕ NetworkBehaviour を継承させることで、空のコールバックを書く必要が一切なくなります
public class PhotonPlayerController : NetworkBehaviour
{
    private Vector2 _latestMove;
    private bool _latestAttack;
    private readonly CompositeDisposable _disposables = new();

    private readonly Subject<Vector2> _onMoveTriggered = new();
    private readonly Subject<Unit> _onAttackTriggered = new();

    private void Start()
    {
        // R3ストリームで最新の入力をキャッシュにキープする
        _onMoveTriggered.Subscribe(val => _latestMove = val).AddTo(_disposables);
        _onAttackTriggered.Subscribe(_ => _latestAttack = true).AddTo(_disposables);
    }

    // ⭕ NetworkBehaviourを継承しているため、正しく Spawned() を override できます
    public override void Spawned()
    {
        // 自分が操作権（Input Authority）を持っているキャラクターのときだけInputSystemを有効化
        if (!HasInputAuthority)
        {
            // 他人の画面のインプット処理は無効化して誤動作を防ぐ
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null) playerInput.enabled = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) => _onMoveTriggered.OnNext(context.ReadValue<Vector2>());
    public void OnAttack(InputAction.CallbackContext context) { if (context.started) _onAttackTriggered.OnNext(Unit.Default); }

    public NetworkInputData GetLocalInput()
    {
        var data = new NetworkInputData 
        { 
            MoveDirection = _latestMove, 
            IsAttack = _latestAttack 
        };
        
        _latestAttack = false; // トリガーは1回読み取ったらリセット
        return data;
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
