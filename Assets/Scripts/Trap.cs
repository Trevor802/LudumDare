using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TileNode
{
    private int cnt = 1;
    public AudioClip dieClip;
    public int trapTimes = 2;// cnt%? 2 is odd/even
    public bool isTrapOnEven = true;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (isTrapOnEven == false)
        {
            animator.Play("TrapOff");
        }
    }
    public override void OnTickStart()
    {
        TrapCount();
    }

    void TrapCount()
    {
        cnt++;
        if (cnt % trapTimes != 0)
        {
            if (isTrapOnEven == true)
            {
                isTrapOnEven = false;
                animator.Play("TrapOff");
            }
            else
            {
                isTrapOnEven = true;
                animator.Play("TrapOn");
            }
        }
        else
        {
            if (isTrapOnEven == false)
            {
                isTrapOnEven = true;
                animator.Play("TrapOn");
            }
            else
            {
                isTrapOnEven = false;
                animator.Play("TrapOff");
            }
        }
    }

    public override void OnPlayerEnter(Player player)
    {
        base.OnPlayerEnter(player);
        if (isTrapOnEven)
        {
            AudioManager.Instance.PlaySingle(dieClip);
            player.Respawn();
        }
    }
}
