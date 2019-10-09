using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    public AudioClip winAudio;
    void Start()
    {
        AudioManager.instance.PlayMusic(winAudio);
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
        UIManager.instance.gameOver = false;
        SceneManager.LoadScene("MainScene");
    }
}
