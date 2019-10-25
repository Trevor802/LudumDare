using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance = null;

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
    }
    #endregion

    public GameObject lifeIcon;
    public RectTransform lifeIconHolder;

    private List<RawImage> m_lifeIcons;
    private List<RawImage> m_crossIcons;

    public void UpdateUI()
    {
        for (int i = 0; i < Player.Instance.initLives; i++)
        {
            m_crossIcons[i].enabled = i >= Player.Instance.GetLives();
        }
    }

    public void ClearUI()
    {
        m_lifeIcons.ForEach(x => x.enabled = false);
        m_crossIcons.ForEach(x => x.enabled = false);
    }

    public void InitUI()
    {
        m_lifeIcons = new List<RawImage>();
        m_crossIcons = new List<RawImage>();
        for (int i = 0; i < Player.Instance.initLives; i++)
        {
            var life = Instantiate(lifeIcon, lifeIconHolder);
            m_lifeIcons.Add(life.GetComponent<RawImage>());
            m_crossIcons.Add(life.transform.GetChild(0).GetComponent<RawImage>());
        }
        m_lifeIcons.ForEach(x => x.enabled = true);
        m_crossIcons.ForEach(x => x.enabled = true);
    }
}
