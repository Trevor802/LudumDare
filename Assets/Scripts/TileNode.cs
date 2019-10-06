using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    public virtual void OnTick() { }
    public virtual void OnPlayerRespawn(Player player) { }
}
