using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaster : MonoBehaviour
{
    // 判定するレイヤー指定
    [SerializeField]
    private LayerMask playerLayer = default;

    // 範囲内に入ったプレイヤを保管
    public List<Player> Players { get; private set; } = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        // プレイヤがトリガー内に侵入
        if ((layerMask & playerLayer) != 0)
        {

            var player = other.transform.root.gameObject.GetComponent<Player>();
            //リストに入っていないプレイヤを追加する
            if (player.isFree)
            {
                // リストに追加
                Players.Add(player);
                player.isFree = false;
                Debug.Log("追加");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        // プレイヤがトリガーから脱出
        if ((layerMask & playerLayer) != 0)
        {
            // プレイヤをリストから削除
            var player = other.transform.root.gameObject.GetComponent<Player>();
            Players.Remove(player);
            player.isFree = true;
            Debug.Log("削除");
        }
    }
}
