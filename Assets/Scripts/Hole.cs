using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hole : TileNode
{
    private Animator animator;
    bool filled = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.Play("pitOpen");
    }
    public override void OnPlayerEnter(Player player)
    {
        if (player)
        {
            if (filled == false)
            {
                print("pass");
                animator.Play("pitClose");
                player.Respawn();
            }
            filled = true;
        }
    }
}