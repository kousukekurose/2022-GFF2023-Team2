using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameObject[] spheres;
    [SerializeField]
    GameObject pauseUI = null;
    [SerializeField]
    Selectable firstSelectable = null;
    
    AudioSource audioSource;
    [SerializeField]
    private AudioClip selectSE = null;
    [SerializeField]
    private AudioClip pauseSE = null;

    private void Start()
    {
        pauseUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = selectSE;
    }

    public void OnPouse(InputAction.CallbackContext context)
    {
        PouseMethod(pauseUI, firstSelectable);
    }

    public void ControllerPouse(GameObject pauseUI, Selectable firstSelectable)
    {
        PouseMethod(pauseUI, firstSelectable);
    }

    private void PouseMethod(GameObject pauseUI, Selectable firstSelectable)
    {
        spheres = GameObject.FindGameObjectsWithTag("PlayerObject");
        if (Time.timeScale == 1f)
        {
            pauseUI.SetActive(true);
            firstSelectable.Select();           
            foreach (var player in spheres)
            {
                player.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }
            Time.timeScale = 0f;
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            foreach (var player in spheres)
            {
                player.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            }
            audioSource.Play();
        }
        
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        spheres = GameObject.FindGameObjectsWithTag("PlayerObject");
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        foreach (var player in spheres)
        {
            player.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        }
        audioSource.Play();
    }
}
