namespace Fusion {
  using System;
  using UnityEngine;
  using System.Collections.Generic;

  /// <summary>
  /// Companion component for <see cref="FusionBootstrap"/>. Automatically added as needed for rendering in-game networking IMGUI.
  /// </summary>
  [RequireComponent(typeof(FusionBootstrap))]
  [AddComponentMenu("Fusion/Fusion Boostrap Debug GUI")]
  [ScriptHelp(BackColor = ScriptHeaderBackColor.Steel)]
  public class FusionBootstrapDebugGUI : Fusion.Behaviour {

    /// <summary>
    /// When enabled, the in-game user interface buttons can be activated with the keys H (Host), S (Server) and C (Client).
    /// </summary>
    [InlineHelp]
    public bool EnableHotkeys;

    /// <summary>
    /// The GUISkin to use as the base for the scalable in-game UI.
    /// </summary>
    [InlineHelp]
    public GUISkin BaseSkin;

    FusionBootstrap _networkDebugStart;
    string _clientCount;
    bool IsMultiplePeerMode => NetworkProjectConfig.Global.PeerMode == NetworkProjectConfig.PeerModes.Multiple;

    Dictionary<FusionBootstrap.Stage, string> _nicifiedStageNames;

#if UNITY_EDITOR

    protected virtual void Reset() {
      _networkDebugStart = EnsureNetworkDebugStartExists();
      _clientCount = _networkDebugStart.AutoClients.ToString();
      BaseSkin = GetAsset<GUISkin>("e59b35dfeb4b6f54e9b2791b2a40a510");
    }

#endif

    protected virtual void OnValidate() {
      ValidateClientCount();
    }

    protected void ValidateClientCount() {
      if (_clientCount == null) {
        _clientCount = "1";
      } else {
        _clientCount = System.Text.RegularExpressions.Regex.Replace(_clientCount, "[^0-9]", "");
      }
    }
    protected int GetClientCount() {
      try {
        return Convert.ToInt32(_clientCount);
      } catch {
        return 0;
      }
    }

    protected virtual void Awake() {

      _nicifiedStageNames = ConvertEnumToNicifiedNameLookup<FusionBootstrap.Stage>("Fusion Status: ");
      _networkDebugStart = EnsureNetworkDebugStartExists();
      _clientCount = _networkDebugStart.AutoClients.ToString();
      ValidateClientCount();
    }

    // ★★★ ⭕ 【変更箇所①】再生した瞬間に、ボタンを押させずに自動判別（早い者勝ち）接続をスタート！ ★★★
    protected virtual void Start() {
      var nds = EnsureNetworkDebugStartExists();
      if (nds == null) return;

      // 2人が確実に同じ部屋に合流できるように部屋名を強制固定
      nds.DefaultRoomName = "2PlayerMatchRoom";

      Debug.Log("[Photon] ゲームが起動されました。ボタンなしで自動的にマッチング（Autoモード）を開始します...");
      
      // 1人目なら自動でホスト(1P)になり、2人目なら自動でクライアント(2P)になります
      if (IsMultiplePeerMode) {
        StartMultipleAutoClients(nds);
      } else {
        nds.StartAutoClient();
      }
    }

    protected FusionBootstrap EnsureNetworkDebugStartExists() {
      if (_networkDebugStart) {
        if (_networkDebugStart.gameObject == gameObject)
          return _networkDebugStart;
      }

      if (TryGetBehaviour<FusionBootstrap>(out var found)) {
        _networkDebugStart = found;
        return found;
      }

      _networkDebugStart = AddBehaviour<FusionBootstrap>();
      return _networkDebugStart;
    }

    private void Update() {

      var nds = EnsureNetworkDebugStartExists();
      if (!nds.ShouldShowGUI) {
        return;
      }

      var currentstage = nds.CurrentStage;
      if (currentstage != FusionBootstrap.Stage.Disconnected) {
        return;
      }

      if (EnableHotkeys) {
        if (Input.GetKeyDown(KeyCode.I)) {
          _networkDebugStart.StartSinglePlayer();
        }

        if (Input.GetKeyDown(KeyCode.H)) {
          if (IsMultiplePeerMode) {
            StartHostWithClients(_networkDebugStart);
          } else {
            _networkDebugStart.StartHost();
          }
        }

        if (Input.GetKeyDown(KeyCode.S)) {
          if (IsMultiplePeerMode) {
            StartServerWithClients(_networkDebugStart);
          } else {
            _networkDebugStart.StartServer();
          }
        }

        if (Input.GetKeyDown(KeyCode.C)) {
          if (IsMultiplePeerMode) {
            StartMultipleClients(nds);
          } else {
            nds.StartClient();
          }
        }

        if (Input.GetKeyDown(KeyCode.A)) {
          if (IsMultiplePeerMode) {
            StartMultipleAutoClients(nds);
          } else {
            nds.StartAutoClient();
          }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
          if (IsMultiplePeerMode) {
            StartMultipleSharedClients(nds);
          } else {
            nds.StartSharedClient();
          }
        }
      }
    }

    // ★★★ ⭕ 【変更箇所②】OnGUIの中身を完全に消去！ ★★★
    protected virtual void OnGUI() {
      // ➔ これにより、画面上の邪魔な黒いパネルやたくさんの開発用ボタンが100%綺麗に消滅します。
      // 画面上には、あなたの作ったゲームマネージャーの「1P待機中...」というカッコいいロビー画面だけが表示されます！
    }

    // --- ⭕ 以下、Photon本来の関数群を1文字も削らずに完全維持（これでエラーが確実に消えます） ---
    private void StartHostWithClients(FusionBootstrap nds) {
      int count;
      try {
        count = Convert.ToInt32(_clientCount);
      } catch {
        count = 0;
      }
      nds.StartHostPlusClients(count);
    }

    private void StartServerWithClients(FusionBootstrap nds) {
      int count;
      try {
        count = Convert.ToInt32(_clientCount);
      } catch {
        count = 0;
      }
      nds.StartServerPlusClients(count);
    }

    private void StartMultipleClients(FusionBootstrap nds) {
      int count;
      try {
        count = Convert.ToInt32(_clientCount);
      } catch {
        count = 0;
      }
      nds.StartMultipleClients(count);
    }

    private void StartMultipleAutoClients(FusionBootstrap nds) {
      int.TryParse(_clientCount, out int count);
      nds.StartMultipleAutoClients(count);
    }

    private void StartMultipleSharedClients(FusionBootstrap nds) {
      int count;
      try {
        count = Convert.ToInt32(_clientCount);
        if (IsMultiplePeerMode) {
          count++; // add 1 for convenience to match host mode
        }
      } catch {
        count = 0;
      }
      nds.StartMultipleSharedClients(count);
    }

    public static Dictionary<T, string> ConvertEnumToNicifiedNameLookup<T>(string prefix = null, Dictionary<T, string> nonalloc = null) where T : System.Enum {

      System.Text.StringBuilder sb = new System.Text.StringBuilder();

      if (nonalloc == null) {
        nonalloc = new Dictionary<T, string>();
      } else {
        nonalloc.Clear();
      }

      var names = Enum.GetNames(typeof(T));
      var values = Enum.GetValues(typeof(T));
      for (int i = 0, cnt = names.Length; i < cnt; ++i) {
        sb.Clear();
        if (prefix != null) {
          sb.Append(prefix);
        }
        var name = names[i];
        for (int n = 0; n < name.Length; n++) {
          if (char.IsUpper(name[n]) == true && n != 0) {
            sb.Append(" ");
          }
          sb.Append(name[n]);
        }
        nonalloc.Add((T)values.GetValue(i), sb.ToString());
      }
      return nonalloc;
    }

#if UNITY_EDITOR
    public static T GetAsset<T>(string Guid) where T : UnityEngine.Object {
      var path = UnityEditor.AssetDatabase.GUIDToAssetPath(Guid);
      if (string.IsNullOrEmpty(path)) {
        return null;
      } else {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
      }
    }
#endif
  }
}
