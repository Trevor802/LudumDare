using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : TileNode
{
    //private GameMaster gm;
    //private GameObject playerObj = GameObject.Find("Player");
    public Vector3 lastCheckPointPos;
    public Player otherPlayer;
    //private Doors door;
    void Start()
    {
       // gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }
    private void Awake()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
       
        Player otherPlayer = other.GetComponent<Player>();
        if (otherPlayer.CompareTag("Player"))
        {
            //lastCheckPointPos[0] = transform.position;

            //otherPlayer.ResetRespawnPos(new Vector3(lastCheckPointPos.x,lastCheckPointPos.y,otherPlayer.transform.position.z));
            lastCheckPointPos = transform.position;
            otherPlayer.ResetRespawnPos(new Vector3(lastCheckPointPos.x, lastCheckPointPos.y, otherPlayer.transform.position.z));
        }
        //Doors door2 = otherDoor.GetComponent<Doors>();
        //if(door2.CompareTag)
        
    }
    
    void Update()
    {
        
        
        
    }
}
