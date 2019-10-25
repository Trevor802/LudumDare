using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    public static CameraManager instance = null;

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

    public List<GameObject> cinema_list;
    public static int level_index;
    // Start is called before the first frame update
    void Start()
    {
        if (!UIManager.instance.gameOver)
        {
            level_index = 0;
        }
        else
        {
            level_index = UIManager.instance.levelIndex;
        }
        for (int i = 0; i < cinema_list.Count; i++)
        {
            if (i == level_index)
                cinema_list[i].SetActive(true);
            else
                cinema_list[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchLevelCamera();
        }
    }

    public void SwitchLevelCamera()
    {
        level_index++;
        level_index %= cinema_list.Count;
        cinema_list.ForEach(cam => cam.SetActive(false));
        cinema_list[level_index].SetActive(true);
    }
}
