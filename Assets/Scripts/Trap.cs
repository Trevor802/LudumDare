﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TileNode
{
   // public bool hitWithTrap = false;
    private int cnt = 0;
    //public GameObject trap;
    public int firstTrapAtStep = 0;//first trap appears in which step
    public int trapTimes = 2;// cnt%? 2 is odd/even
    public bool isTrap = true;
    // Start is called before the first frame update
    void Start()
    {

    }
    override public void OnTick()
    {
        cnt++;
        IsTrap();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void IsTrap()
    {
        for(int i = 0; i < trapTimes - 1; i++)
        {
            if (cnt % trapTimes != 0)
            {

                GetComponent<SpriteRenderer>().enabled = false;
                isTrap = false;
                // this.gameObject.SetActive(false);
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = true;
                isTrap = true;
            }
        }
       

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && isTrap == true)
        {
            //hitWithTrap = true;
            collision.gameObject.GetComponent<Player>().Respawn();
            //this.gameObject.SetActive(false);
        }
    }
}