using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : TileNode
{
    public Vector3 lastCheckPointPos;
    public Player otherPlayer;
    void OnTriggerEnter2D(Collider2D other)
    {
       
        Player otherPlayer = other.GetComponent<Player>();
        if (otherPlayer.CompareTag("Player"))
        {
            lastCheckPointPos = transform.position;
            otherPlayer.ResetRespawnPos(new Vector3(lastCheckPointPos.x, lastCheckPointPos.y, otherPlayer.transform.position.z));
        }
        
    }
    
    void Update()
    {
        
        
        
    }
}
