using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StageScene : MonoBehaviour
{
    [SerializeField]
    private TimeLimit timeUI = null;
    [SerializeField]
    private GameObject playerManager = null;
    [SerializeField]
    private Pause pauseUI = null;
    [SerializeField]
    private ScoreUI scoreUI;

    //[SerializeField]
    //private StageClearUI stageClearUI = null;

    [SerializeField]
    AudioClip stageBGM;
    [SerializeField]
    AudioClip selectSE;
    [SerializeField]
    AudioClip startSE;
    [SerializeField]
    AudioClip stageClearSE;

    [SerializeField]
    GameObject UserReady01 = null;
    [SerializeField]
    GameObject UserReady02 = null;
    [SerializeField]
    GameObject User01 = null;
    [SerializeField]
    GameObject User02 = null;
    [SerializeField]
    GameObject Player01Model01 = null;
    [SerializeField]
    GameObject Player01Model02 = null;
    [SerializeField]
    GameObject Player02Model01 = null;
    [SerializeField]
    GameObject Player02Model02 = null;

    GameObject[] players;

    [SerializeField]
    private GameObject[] hpbox;

    enum SceneState
    {
        Start,
        Play,
        StageClear,
    }
    SceneState gameState = SceneState.Start;

    Animator animator;
    static readonly int fadeOutId = Animator.StringToHash("FadeOut");

    int killScore;
    float time = 0.0f;

    [SerializeField]
    bool isLastScene;

    #region
    public static StageScene Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SoundManager.Instance.PlayBGM(stageBGM);
        gameState = SceneState.Start;

        Player01Model02.SetActive(false);
        Player02Model02.SetActive(false);
        HideAllPlayerUI();
        playerManager.GetComponent<PlayerInputManager>().DisableJoining();
    }

    public void OnPlayGameEvent()
    {
        SoundManager.Instance.PlaySE(startSE);
        gameState = SceneState.Play;
        //timeUI.TimeCount();
        ShowPlayerUI();
        playerManager.GetComponent<PlayerInputManager>().EnableJoining();

        for (int i = 0; i <= 9; i++)
        {
            hpbox[i].SetActive(true);
        }
    }

    #endregion

    #region

    public void StageClear()
    {
        if (gameState == SceneState.Play)
        {
            SoundManager.Instance.PlaySE(stageClearSE);

            SoundManager.Instance.PlaySE(stageClearSE);

            LoadGameClearScene();
        }
    }
    #endregion

    #region �X�V
    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case SceneState.Start:
                break;
            case SceneState.Play:
                break;
            case SceneState.StageClear:
                break;
            default:
                break;
        }
        if (players == null || players.Length <= 1)
        {
            players = GameObject.FindGameObjectsWithTag("PlayerObject");
        }
        else
        {
            if (players.Length != 0)
            {
                if (players[0].transform.GetChild(0).transform.GetComponent<SearchArea>().PlayerTarget)
                {
                    Player01Model01.SetActive(false);
                    Player01Model02.SetActive(true);
                }
                else
                {
                    Player01Model01.SetActive(true);
                    Player01Model02.SetActive(false);
                }
                if (players[1].transform.GetChild(0).transform.GetComponent<SearchArea>().PlayerTarget)
                {
                    Player02Model01.SetActive(false);
                    Player02Model02.SetActive(true);
                }
                else
                {
                    Player02Model01.SetActive(true);
                    Player02Model02.SetActive(false);
                }
            }
        }
    }

    // PlayerUI��\��
    public void HideAllPlayerUI()
    {
        User01.SetActive(false);
        User02.SetActive(false);
        UserReady01.SetActive(false);
        UserReady02.SetActive(false);
    }
    // PlayerUI�\��
    public void ShowPlayerUI()
    {
        UserReady01.SetActive(true);
        UserReady02.SetActive(true);
    }

    #endregion

    #region 
    public void Exit()
    {
        LoadScene("Title");
    }

    //public void Retry()
    //{
    //    LoadScene(SceneManager.GetActiveScene().name);
    //}

    //public void StageSelect()
    //{
    //    LoadScene("StageSelect");
    //}

    public void LoadScene(string sceneName)
    {
        StartCoroutine(OnLoadScene(sceneName));
    }

    IEnumerator OnLoadScene(string sceneName)
    {
        SoundManager.Instance.PlaySE(selectSE);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadGameClearScene()
    {
        StartCoroutine(OnLoadGameClearScene());
    }
    IEnumerator OnLoadGameClearScene()
    {
        SoundManager.Instance.PlaySE(selectSE);
        animator.SetTrigger(fadeOutId);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameClear");
    }

    #endregion
}
