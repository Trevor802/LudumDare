using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hole : TileNode
{
    private Animator animator;
    bool filled = false;
    private AudioSource source;
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        //animator.Play("pitOpen");
    }
    public override void OnPlayerEnter(Player player)
    {
        if (player)
        {
            if (filled == false)
            {
                animator.Play("pitClose");
                source.Play();
                player.Respawn();
            }
            filled = true;
        }
    }
}