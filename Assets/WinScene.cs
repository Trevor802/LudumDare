using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UIManager.instance.ClearUI();
    }
    public void StartAnim()
    {
        Debug.Log("Playing Animation");
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene("MainScene");
    }
}
