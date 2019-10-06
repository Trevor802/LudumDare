using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : TileNode
{
    bool slipping = false;
    Player player;
    void OnTriggerStay2D(Collider2D col)
    {
        player = col.GetComponent<Player>();
        if (player && Vector2.Distance(player.transform.position, transform.position) <= float.Epsilon)
        {
            slipping = true;
        }
    }

    private void Update()
    {
        if (player)
        {
            if (slipping == true && !player.moving)
            {
                RaycastHit2D hit;
                player.Move((int)player.lastMove.x, (int)player.lastMove.y, out hit, false);
                if (!hit.transform)
                {
                    slipping = false;
                }
            }
        }
    }
}