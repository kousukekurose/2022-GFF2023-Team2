using UnityEngine;
using Fusion;


public class AutoGameLauncher : MonoBehaviour
{
    // インスペクターからシーン内の FusionBootstrap を紐付けます
    [SerializeField] private FusionBootstrap _bootstrap;
    
    // 2人が合流するための部屋名（インスペクターから自由に変えられます）
    [SerializeField] private string roomName = "2PlayerMatchRoom";

    /// <summary>
    /// 「ゲームスタート」ボタンを押したときに実行する関数
    /// </summary>
    public void OnClickGameStart()
    {
        if (_bootstrap == null) return;

        // 1. 2人が確実に同じ部屋に合流できるように部屋名を固定する
        _bootstrap.DefaultRoomName = roomName;

        // 2. ★超重要：自動判別モードで接続を開始する★
        // 1人目ならホスト(1P)になり、2人目なら自動でクライアント(2P)になります
        _bootstrap.StartAutoClient();

        // 3. 接続が始まったらボタンを非表示にする
        gameObject.SetActive(false);
    }
}
