using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameMaster gm;
    //private GameObject playerObj = GameObject.Find("Player");
    public Player otherPlayer;
    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Player otherPlayer = other.GetComponent<Player>();
        if (otherPlayer.CompareTag("Player"))
        {
            gm.lastCheckPointPos = transform.position;
            
            otherPlayer.ResetRespawnPos(gm.lastCheckPointPos);
        }
    }
    void Update()
    {
        
        
        
    }
}
