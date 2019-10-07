using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerboardV : TileNode
{
    bool Triggered = false;
    bool PlayerDiedOn = false;
    public string doorName = "door";
    private float threshold = 0.2f;
    private Animator animator;
    private Animator doorAnimator;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        doorAnimator = this.transform.Find(doorName).GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Triggered == true)
        {
            animator.SetBool("PlayerOn", true);
            GameObject door = this.transform.Find(doorName).gameObject;
            door.GetComponent<BoxCollider2D>().enabled = false;
            doorAnimator.Play("DoorOnV");
        }
        if (Triggered == false)
        {
            animator.SetBool("PlayerOn", false);
            GameObject door = this.transform.Find(doorName).gameObject;
            door.GetComponent<BoxCollider2D>().enabled = true;
            doorAnimator.Play("DoorOffV");
        }
    }

    public override void OnPlayerRespawnStart(Player player)
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= threshold)
        {
            PlayerDiedOn = true;
            Triggered = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Triggered = true;
        if (!PlayerDiedOn)
        {
            source.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PlayerDiedOn)
        {
            Triggered = false;
        }
    }

}
