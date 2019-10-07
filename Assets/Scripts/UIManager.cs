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
    }
    #endregion

    public Text stepsText;
    public Text livesText;
    public Text keyText;
    public RawImage[] lifeIcons;
    public RawImage[] crossIcons;
    public void UpdateUI()
    {
        Player player = FindObjectOfType<Player>();
        stepsText.text = "Steps: " + player.steps;
        livesText.text = "Lives: " + player.lives;
        keyText.text = "Key: " + player.hasKey.ToString();
        for (int i = 0; i < player.initLives; i++)
        {
            if (i <= player.lives - 1)
            {
                lifeIcons[i].enabled = true;
                crossIcons[i].enabled = false;
            }
            else
            {
                lifeIcons[i].enabled = false;
                crossIcons[i].enabled = true;
            }
        }
    }

    public void ClearUI()
    {
        stepsText.text = "";
        livesText.text = "";
        keyText.text = "";
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = false;
            crossIcons[i].enabled = false;
        }
    }

    public void initUI()
    {
        Player player = FindObjectOfType<Player>();
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            crossIcons[i].enabled = false;
            if (i <= player.initLives - 1)
            {
                lifeIcons[i].enabled = true;
            }
            else
            {
                lifeIcons[i].enabled = false;
            }
        }
    }
}
