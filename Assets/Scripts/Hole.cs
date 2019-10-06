using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hole : TileNode
{
    bool filled = false;
    public override void OnPlayerEnter(Player player)
    {
        if (player)
        {
            if (filled == false)
            {
                player.Respawn();
            }
            filled = true;
        }
    }
}