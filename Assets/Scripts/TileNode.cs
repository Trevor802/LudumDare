using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileNode : MonoBehaviour
{
    public abstract void OnTick();
    public abstract void OnPlayerDead(Player player);
}
