using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    protected bool playerIn;
    protected Player collidedPlayer;

    public virtual void OnTickStart() { }
    public virtual void OnTickEnd()
    {
        if (playerIn)
        {
            OnPlayerEnter(collidedPlayer);
        }
    }
    public virtual void OnPlayerRespawnStart(Player player)
    {

    }
    public virtual void OnPlayerRespawnEnd(Player player)
    {
        playerIn = false;
    }

    public virtual void OnPlayerEnter(Player player) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collidedPlayer = other.GetComponent<Player>();
        if (collidedPlayer)
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == collidedPlayer)
        {
            playerIn = false;
        }
    }
}
