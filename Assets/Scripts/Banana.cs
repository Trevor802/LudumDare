using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    bool slipping = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        RaycastHit2D hit;
        if (player)
        {
            slipping = true;
            if (slipping == true)
            {
                player.Move((int)player.lastMove.x, (int)player.lastMove.y, out hit, false);
            }
            Vector2 start = player.transform.position;
            Vector2 end = start + player.lastMove;
            hit = Physics2D.Linecast(start, end, player.blockingLayer);
            if (hit.transform != null)
            {
                slipping = false;
            }

        }
    }
}