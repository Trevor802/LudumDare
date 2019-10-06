using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;
    public Player playerPos;
    
    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

    }


    void Update()
    {
       
    }

}
