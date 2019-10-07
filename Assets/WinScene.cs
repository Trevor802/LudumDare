using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    public AudioClip audio;
    void Start()
    {
        AudioManager.instance.PlayMusic(audio);
        StartAnim();
    }

    // Update is called once per frame
    void Update()
    {
        //UIManager.instance.ClearUI();
    }
    public void StartAnim()
    {
        SceneManager.LoadScene("Animation_GuguBorn");
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene("MainScene");
    }
}
