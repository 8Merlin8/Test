using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();  
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1.0f;
        PauseGame = false;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0.0f;
        PauseGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    
    private bool isMuted = false;

    public void ToggleMute()
    {
        Debug.Log("Audio");
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        isMuted = !isMuted;

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = isMuted ? 0f : 1f;
            audioSource.mute = isMuted;
        }
    }

    public Slider volumeSlider;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }

        if (audioSource != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
        else
        {
            Debug.LogWarning("No AudioSource found on MainMenu or in the scene.");
        }
    }

    public void UpdateVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
        else
        {
            Debug.LogWarning("AudioSource is null. Cannot update volume.");
        }
    }
}
