using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void UpdateUI()
    {
        Player player = FindObjectOfType<Player>();
        stepsText.text = "Steps: " + player.steps;
        livesText.text = "Lives: " + player.lives;
        keyText.text = "Key: " + player.hasKey.ToString();
    }
}
