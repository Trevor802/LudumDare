using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerboard : TileNode
{
    bool Triggered = false;
    bool PlayerDiedOn = false;
    public string doorName = "door";
    private float threshold = 0.2f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerInside)
            Triggered = true;
        if (!playerInside && !PlayerDiedOn)
            Triggered = false;
        if (Triggered == true)
        {
            animator.SetBool("PlayerOn", true);
            GameObject door = this.transform.Find(doorName).gameObject;
            door.GetComponent<BoxCollider2D>().enabled = false;
            door.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Triggered == false)
        {
            animator.SetBool("PlayerOn", false);
            GameObject door = this.transform.Find(doorName).gameObject;
            door.GetComponent<BoxCollider2D>().enabled = true;
            door.GetComponent<SpriteRenderer>().enabled = true;
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
}
