using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource bgmAudioSource;
    [SerializeField]
    AudioSource seAudioSource;
    #region 

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public void PlayBGM(AudioClip clip)
    {
        bgmAudioSource.clip = clip;
        if (clip == null)
        {
            return;
        }
        bgmAudioSource.Play();
    }
    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        seAudioSource.PlayOneShot(clip);
    }
    public void ChangeVolume(float bgm, float se)
    {
        bgmAudioSource.volume = bgm;
        seAudioSource.volume = se;
    }
}