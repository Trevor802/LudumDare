using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : TileNode
{ 
    public override void OnPlayerEnter(Player player)
    {
        base.OnPlayerEnter(player);
        player.ResetRespawnPos(new Vector3(transform.position.x,
            transform.position.y, player.transform.position.z));
    }
}
