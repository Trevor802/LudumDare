using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;
    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            transform.position = gm.lastCheckPointPos;
        }
    }
    
}
