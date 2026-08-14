using UnityEngine;
using Fusion;

public class PhotonPlayerStates : NetworkBehaviour
{
    
}

// 待機状態
public class PlayerIdleState : IPhotonPlayerState
{
    public void Enter(PhotonPlayer context)
    {
        Debug.Log("Idle開始");
        context.animator.SetFloat(PhotonPlayer.verticalId, 0f);
    } 
    public void Update(PhotonPlayer context) { }
    public void Exit(PhotonPlayer context) { }

    public void FixedUpdateNetwork(PhotonPlayer context, NetworkInputData inputData)
    {
        if(!inputData.IsAttack)
        {
            // 入力が入ったら即座に移動ステートへ切り替える
            if (inputData.MoveDirection != Vector2.zero)
            {
                context.TransitionTo(new PlayerMoveState());
            }
        }
        else
        {
            context.TransitionTo(new PlayerFireState());
        }
    }
}

// 移動状態
public class PlayerMoveState : IPhotonPlayerState
{
    public void Enter(PhotonPlayer context)
    {
        Debug.Log("Move開始");
        context.animator.SetFloat(PhotonPlayer.verticalId, 1f);

    } 
        public void Update(PhotonPlayer context) { }
    public void Exit(PhotonPlayer context) { }

    public void FixedUpdateNetwork(PhotonPlayer context, NetworkInputData inputData)
    {
        if(!inputData.IsAttack)
        {
            // 入力がなくなったら待機へ戻す
            if (inputData.MoveDirection == Vector2.zero)
            {
                context.Rb.linearVelocity = new Vector3(0f, context.Rb.linearVelocity.y, 0f);
                context.Move(Vector2.zero); 
                context.TransitionTo(new PlayerIdleState());
                return;
            }
        }
        else
        {
            context.TransitionTo(new PlayerFireState());
        }

        // 移動を実行
        context.Move(inputData.MoveDirection);
    }
    
}

public class PlayerFireState : IPhotonPlayerState
{
    public void Enter(PhotonPlayer context)
    {
        Debug.Log("攻撃開始");
        context.animator.SetTrigger(PhotonPlayer.FireId);
    }
    public void FixedUpdateNetwork(PhotonPlayer context, NetworkInputData inputData){}
    public void Update(PhotonPlayer context)
    {
        if(context.animator != null)
        {
            var stateInfo = context.animator.GetCurrentAnimatorStateInfo(0);
        
        　 // ⭕ あなたのアニメーションステート名「FixAttack(Fire)」をそのまま指定します！
            if (stateInfo.IsName("FixAttack(Fire)") && stateInfo.normalizedTime >= 1.0f)
            {
                // アニメーションが出し切られたら、安全に待機状態（Idle）に戻す
                context.TransitionTo(new PlayerIdleState());
            }
        }
    }
    public void Exit(PhotonPlayer context)
    {
        Debug.Log("攻撃終了");
        context.UnfreezePhysics();
    }

}

