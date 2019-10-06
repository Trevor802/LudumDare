using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    bool filled = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        if (player)
        {
            if(filled == false)
            {
                player.Respawn();
            }

            filled = true;
        }
    }
}
