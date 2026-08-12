using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    // 獲得数を表示するTextを指定します。
    [SerializeField]
    private TextMeshProUGUI textUI = null;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("player01Score", 0);
        //UpdateValue();
    }
    private void Update()
    {
        textUI.text = string.Format("{0}", score);
    }
    // UIを表示更新します。
    public void UpdateValue()
    {
        textUI.text = string.Format("{0}", score);
        //textUI.text = $"{StageScene.Instance.ScoreNum:d04}";
    }
}
