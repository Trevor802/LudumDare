using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma : TileNode
{
    private SpriteRenderer magmaInv;
    private int magmaNum = 0;
    private bool ifMagma = false;
    public void Start()
    {
        magmaInv =gameObject.GetComponent<SpriteRenderer>();
        magmaInv.enabled = false;
    }
    public override void OnPlayerExit(Player player)
    {
        magmaInv.enabled = true;
        ifMagma = true;
        
    }
    public override void OnTickStart()
    {
        if(ifMagma ==true)
        {
            magmaCount();
        }
        
    }
    void magmaCount()
    {
        magmaNum++;
    }
    private void Update()
    {
        if(magmaNum >1)
        {
            gameObject.SetActive(false);
        }
    }
    public override void OnPlayerEnter(Player player)
    {
        if(ifMagma ==true)
        {
            player.Respawn();
        }
        

    }
}
