using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    float animationTime = 1;
    [SerializeField]
    Selectable firstSelectTitle = null;
    [SerializeField]
    Selectable firstSelectTutorial = null;

    AudioSource audioSource;
    [SerializeField]
    AudioClip selectSE;
    [SerializeField]
    AudioClip BGM;
    [SerializeField]
    private AudioClip sound = null;
    static readonly int fadeOutId = Animator.StringToHash("FadeOut");
    static readonly int tutorialId = Animator.StringToHash("Tutorial");

    private void Start()
    {
        animator = GetComponent<Animator>();
        SoundManager.Instance.PlayBGM(BGM);
        audioSource = GetComponent<AudioSource>();
    }
    public void ShowTutorial()
    {
        SoundManager.Instance.PlaySE(selectSE);
        animator.SetBool(tutorialId, true);
        firstSelectTutorial.Select();
    }
    public void HideTutorial()
    {
        SoundManager.Instance.PlaySE(selectSE);
        animator.SetBool(tutorialId, false);
        firstSelectTitle.Select();
    }
    public void LoadNextStage()
    {
        SoundManager.Instance.PlaySE(selectSE);
        StartCoroutine(OnLoadNextScene());
    }

    public void Sound()
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
    IEnumerator OnLoadNextScene()
    {
        animator.SetTrigger(fadeOutId);
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("Stage2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}