using UnityEngine;
using Fusion;

public class PhotonPlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [Header("🤖 デバッグ・テスト設定")]
    [SerializeField] private bool spawnDummyPlayer = false; //インスペクターでチェックを入れると疑似2Pが生まれます
    [SerializeField] private Vector3 dummySpawnPosition = new Vector3(2f, 1f, 0f); // 疑似2Pの出現位置

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

            if (spawnDummyPlayer)
            {
                SpawnTestingDummy();
            }
        
        }
    }

    /// <summary>
    /// ⭕ テスト用の疑似2P（ダミー人形）をネットワーク上にスポーンさせる関数
    /// </summary>
    private void SpawnTestingDummy()
    {
        Debug.Log("[Debug] ⚠️テスト用の疑似2P（ダミー人形）を生成します。");

        // ⚠️ 第4引数（inputAuthority）に「null」を指定するのが最大のポイント！
        // 誰のキーボード入力にも反応しない「完全なサンドバッグ状態の人形」が生まれます。
        NetworkObject dummyObj = Runner.Spawn(PlayerPrefab, dummySpawnPosition, Quaternion.identity, inputAuthority: null);

        if (dummyObj.TryGetComponent<PhotonPlayer>(out var dummyPlayer))
        {
            // ➔ 人形のプレイヤー番号を「2（2P）」に強制設定！
            // これにより、以前作ったシステムが自動連動し、全身が「2P用のマテリアル（赤色など）」に勝手に変化します。
            dummyPlayer.SetPlayerNumber(2);

            // ➔ ゲームマネージャーにも「2Pが合流したぞ！」と認識させます。
            // これにより、この人形が生まれた瞬間にロビーUIの「2P参加」がパッと点灯し、
            // 「3, 2, 1... FIGHT!!」のカウントダウン演出が1人でも100%完全に動き出します！
            if (photonGameManager != null)
            {
                photonGameManager.OnPlayerSpawned(dummyPlayer);
            }
        }
    }
}
