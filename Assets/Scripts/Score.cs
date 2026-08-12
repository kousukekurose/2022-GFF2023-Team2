using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    //�����|�C���g
    int player01Score = 0;
    int player02Score = 0;
    int type = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (this.name == "killScoreUI01")
        {
            type = 1;
            PlayerPrefs.SetInt("player01Score", player01Score);
        }
        else if (this.name == "killScoreUI02")
        {
            type = 2;
            PlayerPrefs.SetInt("player02Score", player02Score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == 1)
        {
            player01Score = PlayerPrefs.GetInt("player01Score", 0);
            scoreUI.text = string.Format("{0}", player01Score);
        }
        else if (type == 2)
        {
            player02Score = PlayerPrefs.GetInt("player02Score", 0);
            scoreUI.text = string.Format("{0}", player02Score);
        }
    }
    public void ScorePus(int type,int score)
    {
        Debug.Log("type:" + type);
        if (type == 1)
        {
            player01Score += score;
            PlayerPrefs.SetInt("player01Score", player01Score);
        }
        else if (type == 2)
        {
            player02Score += score;
            PlayerPrefs.SetInt("player02Score", player02Score);
        }
    }
}
