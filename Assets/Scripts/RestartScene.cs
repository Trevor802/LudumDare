using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
    
public class RestartScene : MonoBehaviour
{
    public GameObject obj;

    public void ShowText()
    {
        obj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            VII.SceneManager.Instance.LoadScene(VII.SceneType.GameScene);
        }
    }
}
