using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("---------- Chapter Clips ----------")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    public AudioClip outroMusic;

    [Header("---------- SFX Clips ----------")]
    public AudioClip boing4; // Arabadan Sekme done
    public AudioClip click1; // Klasik click sesi 
    public AudioClip hurt1; // Player Die() done
    public AudioClip pop1; // Mushroom bounce done
    public AudioClip jumpWhoosh1; // Jump() done
    public AudioClip binbanbon; // Cutscene 4
    public AudioClip textScrool; // Ahmet ve Emir Speech
    public AudioClip textScroolClown; // CarScene Gülse

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic(mainMenuMusic);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Adjust the music which we want to play in chapters.
        switch (scene.name)
        {
            case "MainMenu":
                SetBackgroundMusic(mainMenuMusic);
                break;
            case "BeytepeScene":
                SetBackgroundMusic(gameplayMusic);
                break;
            case "SpecialThanks":
                SetBackgroundMusic(outroMusic);
                break;
            case "Cutscene4":
                SetBackgroundMusic(null);
                break;
            default:
                SetBackgroundMusic(null);
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetBackgroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
