using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerboard : TileNode
{
    bool Triggered = false;
    bool PlayerDiedOn = false;
    public string doorName = "door";
    private float threshold = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>())
        {
            Triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!PlayerDiedOn)
        {
            Triggered = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Triggered == true)
        {
            GameObject door = GameObject.Find(doorName);
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (Triggered == false)
        {
            GameObject door = GameObject.Find(doorName);
            door.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public override void OnPlayerRespawn(Player player)
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= threshold)
        {
            PlayerDiedOn = true;
            Triggered = true;
        }
    }
}
