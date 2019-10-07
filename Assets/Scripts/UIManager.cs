using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    public Text stepsText;
    public Text livesText;
    public Text keyText;
    public RawImage lifeImage1;
    public RawImage lifeImage2;
    public RawImage lifeImage3;
    public void UpdateUI()
    {
        Player player = FindObjectOfType<Player>();
        stepsText.text = "Steps: " + player.steps;
        livesText.text = "Lives: " + player.lives;
        keyText.text = "Key: " + player.hasKey.ToString();
        /*
        if (player.lives == 3)
        {
            lifeImage1.enabled = false;
            lifeImage2.enabled = false;
            lifeImage3.enabled = true;
        }
        else if (player.lives == 2)
        {
            lifeImage1.enabled = false;
            lifeImage2.enabled = true;
            lifeImage3.enabled = false;
        }
        else if (player.lives == 1)
        {
            lifeImage1.enabled = true;
            lifeImage2.enabled = false;
            lifeImage3.enabled = false;
        }*/
    }

    public void ClearUI()
    {
        stepsText.text = "";
        livesText.text = "";
        keyText.text = "";
        //lifeImage1.enabled = false;
    }

    public void initUI()
    {
        //lifeImage3.enabled = true;
    }
}
