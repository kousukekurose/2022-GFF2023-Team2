using System.Collections;
using UnityEngine;


public class MoveBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    [SerializeField]
    private float speed = 700;

    [SerializeField]
    private int run = 60;

    [SerializeField]
    GameObject player = null;

    [SerializeField]
    EMPArea empArea = null;

    [SerializeField]
    PutShot putShot = null;

    [SerializeField]
    Animator attackUpItemAnimator = null;
    [SerializeField]
    Animator speedUpItemAnimator = null;
    [SerializeField]
    Animator shieldItemAnimator = null;
    [SerializeField]
    Animator spareBatteryItemAnimator = null;

    private Vector3 player_pos;

    private bool OnAvoidance = true;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip playerAttackSE = null;
    [SerializeField]
    private AudioClip playerStanSE = null;
    [SerializeField]
    private AudioClip useItemSE = null;
    [SerializeField]
    private AudioClip dashSE = null;

    enum MotionState
    {
        Locomotion,
        Fire,
        Avoidance,
        Stan,
    }
    private MotionState currentState = MotionState.Locomotion;

    [SerializeField]
    private Animator animator = null;
    private int baseLayer;
    //static readonly int locomotionHash = Animator.StringToHash("Base Layer.Locomotion");
    static readonly int verticalId = Animator.StringToHash("Vertical");
    static readonly int horizontalId = Animator.StringToHash("Horizontal");
    static readonly int FireId = Animator.StringToHash("Fire");
    static readonly int AvoidanceId = Animator.StringToHash("Avoidance");
    static readonly int stanId = Animator.StringToHash("Stan");
    static readonly int attackUpId = Animator.StringToHash("AttackUp");
    static readonly int speedUpId = Animator.StringToHash("SpeedUp");
    static readonly int shieldId = Animator.StringToHash("Shield");
    static readonly int spareBatteryId = Animator.StringToHash("SpareBattery");

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        player_pos = player.transform.position;
    }

    public void Move(Vector3 motion)
    {
        switch (currentState)
        {
            case MotionState.Locomotion:
                {
                    animator.SetFloat(verticalId, Mathf.Abs(motion.z));
                    animator.SetFloat(horizontalId, Mathf.Abs(motion.x));
                    var velocity = transform.forward * motion.z + transform.right * motion.x;
                    velocity.Normalize();
                    velocity = velocity * speed;
                    rigidbody.linearVelocity = velocity;

                    var diff = player.transform.position - player_pos;
                    diff.y = 0;
                    if (diff.magnitude > 0.01)
                    {
                        player.transform.rotation = Quaternion.LookRotation(diff);
                    }
                    player_pos = player.transform.position;
                }
                break;
            default:
                break;
        }
    }

    public void Fire()
    {
        switch (currentState)
        {
            case MotionState.Locomotion:
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    currentState = MotionState.Fire;
                    animator.SetTrigger(FireId);
                    audioSource.clip = playerAttackSE;
                    audioSource.Play();
                }
                break;
        }
    }

    public void Avoidance()
    {
        if (OnAvoidance == true)
        {
            switch (currentState)
            {
                case MotionState.Locomotion:
                    {
                        animator.SetTrigger(AvoidanceId);
                    }
                    break;
            }
        }
    }

    public void Stan()
    {
        switch (currentState)
        {
            case MotionState.Locomotion:
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    currentState = MotionState.Stan;
                    animator.SetTrigger(stanId);
                    audioSource.clip = playerStanSE;
                    audioSource.Play();
                }
                break;
        }
    }

    public void UnStan()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        currentState = MotionState.Locomotion;
    }

    public void StateFireToLocomotion()
    {
        if (currentState == MotionState.Fire)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            currentState = MotionState.Locomotion;
        }
    }

    public void StateAvoidanceToLocomotion()
    {
        //if (currentState == MotionState.Avoidance)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            currentState = MotionState.Locomotion;
            OnAvoidance = false;
            Invoke("CoolTime", 1.0f);
        }
    }

    public void CoolTime()
    {
        OnAvoidance = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MotionState.Locomotion:
                UpdateForLocomotion();
                break;
            default:
                break;
        }
    }

    public void dash()
    {
        speed = run;
        audioSource.clip = dashSE;
        audioSource.Play();
    }
    public void walk()
    {
        speed = 10;
    }

    void UpdateForLocomotion()
    {

    }

    public void UserItem(Item item)
    {
        switch (item.no)
        {
            case 1:
                UseShieldItem(item);
                break;
            case 2:
                UsePowerItem(item);
                break;
            case 3:
                UseClockupItem(item);
                break;
            case 4:
                UseShotItem();
                break;
            case 5:
                UseHeelItem(item);
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            default:
                break;
        }
    }

    private void UseShieldItem(Item item)
    {
        StartCoroutine(OnUseShieldItem(item));
        audioSource.clip = useItemSE;
        audioSource.Play();
    }

    IEnumerator OnUseShieldItem(Item item)
    {
        shieldItemAnimator.SetTrigger(shieldId);
        transform.GetChild(0).transform.GetComponent<PlayerHp>().Decision();
        yield return new WaitForSeconds(item.buffTime);
        transform.GetChild(0).transform.GetComponent<PlayerHp>().Cancellation();
    }

    private void UsePowerItem(Item item)
    {
        attackUpItemAnimator.SetBool(attackUpId, true);
        transform.GetChild(0).transform.GetComponent<SearchArea>().PowerUp(item.attackBuff, item.buffCount);
        audioSource.clip = useItemSE;
        audioSource.Play();
    }
    public void StopPowerItemAnimation()
    {
        attackUpItemAnimator.SetBool(attackUpId, false);
    }

    private void UseClockupItem(Item item)
    {
        speedUpItemAnimator.SetTrigger(speedUpId);
        float tmpSpeed = speed;
        speed = item.speedBuff;
        StartCoroutine(OnUseClockupItem(item, tmpSpeed));
        audioSource.clip = useItemSE;
        audioSource.Play();
    }
    IEnumerator OnUseClockupItem(Item item, float tmpSpeed)
    {
        yield return new WaitForSeconds(item.buffTime);
        speed = tmpSpeed;
    }

    private void UseHeelItem(Item item)
    {
        spareBatteryItemAnimator.SetTrigger(spareBatteryId);
        transform.GetChild(0).transform.GetComponent<PlayerHp>().HpHeel();
        audioSource.clip = useItemSE;
        audioSource.Play();
    }

    private void UseShotItem()
    {
        putShot.PutShotItem();
        audioSource.clip = useItemSE;
        audioSource.Play();
    }
    //private void UseEMPItem()
    //{
    //    StartCoroutine(OnUseEMPItem());
    //}
    //IEnumerator OnUseEMPItem()
    //{
    //    empArea.isEMP = true;
    //    yield return new WaitForSeconds(0.3f);
    //    empArea.isEMP = false;
    //}
}
