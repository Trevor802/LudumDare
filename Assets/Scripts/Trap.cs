using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TileNode
{
    public int cnt = 1;
    public AudioClip dieClip;
    private int trapTimes = 2;// cnt%? 2 is odd/even
    public bool isTrapOnEven = true;
    private bool activated;
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
    override public void OnTickStart()
    {
        cnt++;
        IsTrap();
        activated = false;
    }
    override public void OnTickEnd()
    {
        activated = true;
    }

    void IsTrap()
    {
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

    //TODO Use Tick
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && isTrapOnEven == true && activated)
        {
            AudioManager.instance.PlaySingle(dieClip);
            collision.gameObject.GetComponent<Player>().Respawn();
            activated = false;
        }
    }
}
