using Fusion;

public interface IPhotonPlayerState
{
    void Enter(PhotonPlayer context);
    void FixedUpdateNetwork(PhotonPlayer context, NetworkInputData inputData);
    void Update(PhotonPlayer context);
    void Exit(PhotonPlayer context);
}
