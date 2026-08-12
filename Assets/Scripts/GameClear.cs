using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    float animationTime = 1;

    int player01Score = 0;
    int player02Score = 0;

    [SerializeField]
    GameObject player1Win = null;
    [SerializeField]
    GameObject player1Loss = null;
    [SerializeField]
    GameObject player1Draw = null;
    [SerializeField]
    GameObject Player1Camerawin = null;

    [SerializeField]
    GameObject player2Win = null;
    [SerializeField]
    GameObject player2Loss = null;
    [SerializeField]
    GameObject player2Draw = null;
    [SerializeField]
    GameObject Player2Camerawin = null;


    [SerializeField]
    TextMeshProUGUI judgementText01 = null;
    [SerializeField]
    TextMeshProUGUI judgementText02 = null;
    [SerializeField]
    TextMeshProUGUI scoreTxet01 = null;
    [SerializeField]
    TextMeshProUGUI scoreTxet02 = null;

    [SerializeField]
    AudioClip BGM;
    [SerializeField]
    AudioClip selectSE;

    static readonly int fadeOutId = Animator.StringToHash("FadeOut");

    private void Start()
    {
        SoundManager.Instance.PlayBGM(BGM);
        animator = GetComponent<Animator>();

        player1Win.SetActive(false);
        player1Loss.SetActive(false);
        player1Draw.SetActive(false);
        Player1Camerawin.SetActive(false);

        player2Win.SetActive(false);
        player2Loss.SetActive(false);
        player2Draw.SetActive(false);
        Player2Camerawin.SetActive(false);

        player01Score = PlayerPrefs.GetInt("player01Score");
        player02Score = PlayerPrefs.GetInt("player02Score");
        // �X�R�A�\��
        scoreTxet01.text = string.Format("{0}", player01Score);
        scoreTxet02.text = string.Format("{0}", player02Score);
        Judgement();

    }

    private void Judgement()
    {
        if (player01Score == player02Score)
        {
            player1Draw.SetActive(true);
            player2Draw.SetActive(true);

            //judgementText01.text = string.Format("DRAW");
            //judgementText02.text = string.Format("DRAW");
        }
        else if (player01Score > player02Score)
        {
            Debug.Log("01����");
            Player1Camerawin.SetActive(true);
            player1Win.SetActive(true);
            player2Loss.SetActive(true);

            //judgementText01.text = string.Format("WINNER!");
            //judgementText02.text = string.Format("LOSER");
        }
        else if (player01Score < player02Score)
        {
            Debug.Log("02����");
            Player2Camerawin.SetActive(true);
            player2Win.SetActive(true);
            player1Loss.SetActive(true);

            //judgementText01.text = string.Format("LOSER");
            //judgementText02.text = string.Format("WINNER!");
        }
    }

    public void LoadTitle()
    {
        StartCoroutine(OnLoadTitleScene());
    }
    IEnumerator OnLoadTitleScene()
    {
        SoundManager.Instance.PlaySE(selectSE);
        animator.SetTrigger(fadeOutId);
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("Title");
    }

    public void LoadStage()
    {
        StartCoroutine(OnLoadStageScene());
    }

    IEnumerator OnLoadStageScene()
    {
        SoundManager.Instance.PlaySE(selectSE);
        animator.SetTrigger(fadeOutId);
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("Stage2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
