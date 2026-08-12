using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeLimit : MonoBehaviour
{
    public TextMeshProUGUI timeLabel;

    public int inputTime;
    private int m; 
    private int s;

    private int num;

    [SerializeField]
    int bossGenerateTime = 35;

    [SerializeField]
    BossGenerator bossGenerator = null;

    bool isBoss = true;

    void Start()
    {
        num = inputTime;
        StartCoroutine(TimeCount());
    }

    private IEnumerator TimeCount()
    {
        for (int i = 0; i < num; i++)
        {
            inputTime -= 1;

            int h = inputTime / 3600;
            m = (inputTime - 3600 * h) / 60;
            s = (inputTime - 3600 * h) % 60;

            timeLabel.text = string.Format("{0:0}:{1:00}", m, s);

            yield return new WaitForSeconds(1f);

            if (inputTime <= bossGenerateTime && isBoss)
            {
                bossGenerator.BossGenerate();
                isBoss = false;
            }
            TimeOver();
        }
    }

    public void TimeOver()
    {
        if (inputTime <= 1)
        {

            StageScene.Instance.LoadGameClearScene();
            Debug.Log("0000");
        }
    }
}
