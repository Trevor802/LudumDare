using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
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

    private void Awake()
    {

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
        //Debug.Log(cinema_list.Count+" i="+level_index);

        if(level_index < cinema_list.Count - 1)
        {
            cinema_list[level_index].SetActive(false);
            level_index++;
            cinema_list[level_index].SetActive(true);
        }
        else
        {
            cinema_list[0].SetActive(true);
            cinema_list[level_index].SetActive(false);
            level_index = 0;
        }
        //reset trap cnt when switch level
        for (int i = 0; i < FindObjectsOfType<Trap>().Length; i++)
        {
            FindObjectsOfType<Trap>()[i].cnt = 1;
        }

    }
}
