using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoard : TileNode
{
    public AudioClip triggerClip;
    private bool playerOn = false;
    private bool tombstoneOn;
    public Door targetDoor;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnPlayerEnter(Player player)
    {
        base.OnPlayerEnter(player);
        if (tombstoneOn) return;
        playerOn = true;
        animator.SetBool("PlayerOn", true);
        AudioManager.Instance.PlaySingle(triggerClip);
        targetDoor.OpenDoor();
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        if (playerOn && !tombstoneOn)
        {
            tombstoneOn = true;
        }
    }

    public override void OnPlayerExit(Player player)
    {
        base.OnPlayerExit(player);
        if (tombstoneOn) return;
        playerOn = false;
        animator.SetBool("PlayerOn", false);
        AudioManager.Instance.PlaySingle(triggerClip);
        targetDoor.CloseDoor();
    }

}
