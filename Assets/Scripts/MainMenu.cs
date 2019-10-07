using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartAnim()
    {
        Debug.Log("Playing Animation");
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene("2DLevel");
    }
}
