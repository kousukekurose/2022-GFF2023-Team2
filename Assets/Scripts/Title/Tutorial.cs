using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject[] tutorialImages;
    int nowPage = 0;
    private void Start()
    {
        NowPageActive();
    }
    public void NextTutorial()
    {
        // 前のページを非表示
        tutorialImages[nowPage].SetActive(false);
        nowPage++;
        if (tutorialImages.Length - 1 < nowPage)
        {
            nowPage -= tutorialImages.Length;
        }
        // 今のページを表示
        tutorialImages[nowPage].SetActive(true);
    }
    public void BackTutorial()
    {
        // 前のページを非表示
        tutorialImages[nowPage].SetActive(false);
        nowPage--;
        if (nowPage < 0)
        {
            nowPage += tutorialImages.Length;
        }
        // 今のページを表示
        tutorialImages[nowPage].SetActive(true);
    }
    public void AllPageActive()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(true);
        }
    }
    public void NowPageActive()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(false);
        }
        tutorialImages[nowPage].SetActive(true);
    }
}
