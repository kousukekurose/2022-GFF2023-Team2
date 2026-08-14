using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using R3;

public class PhotonPlayer : NetworkBehaviour,IDamageable
{
    private IPhotonPlayerState _currentState;

    private readonly ReactiveProperty<int> _currentHp = new(5);
    public ReadOnlyReactiveProperty<int> CurrentHp => _currentHp;
    [SerializeField]
    private LayerMask[] layerMask;

    [Networked] public int NetworkHp { get; set; } = 5;
    public int damage = 1;
    public Rigidbody Rb { get; private set; }

    [Networked, OnChangedRender(nameof(OnPlayerIdChanged))]
    public int PlayerId { get; set; }
    [SerializeField] private Material[] playerMaterial;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderer;
    
    [SerializeField] private float moveSpeed = 5f;
    private PhotonPlayerController _controller; // ⭕ コントローラーの参照
    public Animator animator{ get; private set; }

    public static readonly int verticalId = Animator.StringToHash("Vertical");
    public static readonly int horizontalId = Animator.StringToHash("Horizontal");
    public static readonly int FireId = Animator.StringToHash("Fire");
    public static readonly int AvoidanceId = Animator.StringToHash("Avoidance");
    public static readonly int stanId = Animator.StringToHash("Stan");
    public static readonly int attackUpId = Animator.StringToHash("AttackUp");
    public static readonly int speedUpId = Animator.StringToHash("SpeedUp");
    public static readonly int shieldId = Animator.StringToHash("Shield");
    public static readonly int spareBatteryId = Animator.StringToHash("SpareBattery");


    public void SetPlayerNumber(int number)
    {
        if(HasStateAuthority)
        {
            PlayerId = number;
        }
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<PhotonPlayerController>();
    }

    public override void Spawned()
    {
        TransitionTo(new PlayerIdleState());
        NetworkHp = 5;
        _currentHp.Value = NetworkHp;
    }

    public override void FixedUpdateNetwork()
    {
        NetworkInputData inputData = default;

        // ⭕ 自分が操作しているローカル画面なら、R3コントローラーから生の入力を直接もらう
        if (HasInputAuthority)
        {
            if (_controller != null)
            {
                inputData = _controller.GetLocalInput();
            }
        }
        
        // ⭕ Fusionがこの入力データを全画面にパケットとして自動転送してくれます
        // そのデータをステートに流し込む（これで自分も他人も同じタイミングで動く！）
        _currentState?.FixedUpdateNetwork(this, inputData);
    }

    public override void Render()
    {
        _currentState?.Update(this);
    }

    public void TransitionTo(IPhotonPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    public void Move(Vector2 direction)
    {
        if (!HasStateAuthority) return;
        Vector3 targetVelocity = new Vector3(direction.x * moveSpeed, Rb.linearVelocity.y, direction.y * moveSpeed);
        Vector3 lookDirection = new Vector3(targetVelocity.x,0,targetVelocity.z);
        if(lookDirection != Vector3.zero)
        {
            Quaternion baseRotation = Quaternion.LookRotation(lookDirection);
            //元々のモデルが180度回転しているので元に戻す。
            Quaternion finalRotation = baseRotation * Quaternion.Euler(0, 180, 0);
            Rb.linearVelocity = targetVelocity;
            transform.rotation = finalRotation;
        }
    }

    public void Fire()
    {
        if (!HasStateAuthority) return;
        Rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void TkeDamage(int damage)
    {
        NetworkHp -= damage;
        if(NetworkHp == 0)
        {
            
        }
        _currentHp.Value = NetworkHp;
    }

    public void UnfreezePhysics()
    {
        if (!HasStateAuthority) return;

        // キャラクターがバタッと倒れないように、回転（Rotation）だけを固定した元の通常状態に戻します！
        Rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnPlayerIdChanged()
    {
        // もし2人目（2P）だったら、インスペクターで設定した2P用マテリアルに差し替える
        if (PlayerId == 2 && playerMaterial[1] != null && skinnedMeshRenderer != null)
        {
            for(int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = playerMaterial[1];
            }
            Debug.Log($"プレイヤー{PlayerId}の見た目を2P用に変更しました。");
        }
        else
        {
            for(int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = playerMaterial[0];
            }
            Debug.Log($"プレイヤー{PlayerId}の見た目1P用に変更しました。");
        }
    }
}

