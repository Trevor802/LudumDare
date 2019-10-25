using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    public AudioSource musicSource;
    public AudioSource soundSource;

    // Play sound one time
    public void PlaySingle(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        soundSource.clip = clip;
        soundSource.Play();
    }

    // Change the background music
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        musicSource.clip = clip;
        musicSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
