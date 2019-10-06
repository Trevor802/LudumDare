using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<GameObject> cinema_list;
    private int level_index;
    // Start is called before the first frame update
    void Start()
    {
        level_index = 0;
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
        Debug.Log(cinema_list.Count+" i="+level_index);

        if(level_index < cinema_list.Count - 1)
        {
            cinema_list[level_index + 1].SetActive(true);
            cinema_list[level_index].SetActive(false);
            level_index++;
        }
        else
        {
            cinema_list[0].SetActive(true);
            cinema_list[level_index].SetActive(false);
            level_index = 0;
        }
        
    }
}
