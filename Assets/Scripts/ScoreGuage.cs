using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreGuage : MonoBehaviour
{
    [SerializeField]
    Image playerScoreGauge01 = null;
    [SerializeField]
    Image playerScoreGauge02 = null;

    float playerScore01 = 0;
    float playerScore02 = 0;

    // �X�R�A���v�l
    float sum = 0;

    // Update is called once per frame
    void Update()
    {
        ScoreFillAmount();
    }

    private void ScoreFillAmount()
    {
        playerScore01 = PlayerPrefs.GetInt("player01Score");
        playerScore02 = PlayerPrefs.GetInt("player02Score");
        if (sum != 0)
        {
            Debug.Log("1:" + playerScore01 / sum);
            Debug.Log("2:" + playerScore02 / sum);
            playerScoreGauge01.fillAmount = playerScore01 / sum;
            playerScoreGauge02.fillAmount = playerScore02 / sum;
        }
        else
        {
            // 50:50
            playerScoreGauge01.fillAmount = 0.5f;
            playerScoreGauge02.fillAmount = 0.5f;
        }
    }
}
