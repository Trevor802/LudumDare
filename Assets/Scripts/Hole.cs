using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hole : TileNode
{
    private Animator animator;
    bool filled = false;
    public AudioClip holeClose;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override void OnPlayerEnter(Player player)
    {
        if (player)
        {
            if (filled == false)
            {
                animator.Play("pitClose");
                AudioManager.instance.PlaySingle(holeClose);
                player.Respawn();
            }
            filled = true;
        }
    }
}