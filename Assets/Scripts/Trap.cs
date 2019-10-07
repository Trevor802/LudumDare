using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TileNode
{
    // public bool hitWithTrap = false;
    private int cnt = 1;
    //public GameObject trap;
    // private int firstTrapAtStep = 0;
    private int trapTimes = 2;// cnt%? 2 is odd/even
    public bool isTrapOnEven = true;
    private bool activated;
    private Animator animator;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (isTrapOnEven == false)
        {
            animator.Play("TrapOff");
            UIManager.instance.UpdateUI();
            // GetComponent<SpriteRenderer>().enabled = false;
        }
        source = GetComponent<AudioSource>();

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
    // Update is called once per frame
    void Update()
    {

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

            // GetComponent<SpriteRenderer>().enabled = isTrapOnEven;
            // this.gameObject.SetActive(false);
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
            //GetComponent<SpriteRenderer>().enabled = isTrapOnEven;
        }

        UIManager.instance.UpdateUI();
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && isTrapOnEven == true && activated)
        {
            //hitWithTrap = true;
            source.Play();
            collision.gameObject.GetComponent<Player>().Respawn();
            activated = false;
            //this.gameObject.SetActive(false);
        }
    }
}
