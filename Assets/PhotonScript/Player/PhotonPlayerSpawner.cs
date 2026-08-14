using UnityEngine;
using Fusion;

public class PhotonPlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    [SerializeField] PhotonGameManager photonGameManager;

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedObj = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity,player);
            if(spawnedObj.TryGetComponent<PhotonPlayer>(out var photonPlayer))
            {
                photonPlayer.SetPlayerNumber(player.PlayerId);
                photonGameManager.OnPlayerSpawned(photonPlayer);
            }
        
        }
    }
}
