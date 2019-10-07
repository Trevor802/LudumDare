using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    private bool playerInTemp;
    private bool playerOutTemp;
    protected bool playerInside;
    protected Player collidedPlayer;

    public virtual void OnTickStart() { }
    public virtual void OnTickEnd()
    {
        if (playerInTemp)
        {
            playerInside = true;
            OnPlayerEnter(collidedPlayer);
            playerInTemp = false;
        }
        if (playerOutTemp)
        {
            playerInside = false;
            OnPlayerExit(collidedPlayer);
            playerOutTemp = false;
        }
    }
    public virtual void OnPlayerRespawnStart(Player player)
    {
        
    }
    public virtual void OnPlayerRespawnEnd(Player player)
    {
        playerInTemp = false;
        playerOutTemp = false;
        playerInside = false;
    }

    public virtual void OnPlayerEnter(Player player) { }

    public virtual void OnPlayerExit(Player player) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collidedPlayer = other.GetComponent<Player>();
        if (collidedPlayer && !playerInside)
        {
            playerInTemp = true;
            playerOutTemp = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (collidedPlayer && collidedPlayer == other.GetComponent<Player>() && playerInside)
        {
            playerOutTemp = true;
            playerInTemp = false;
        }
    }
}
