using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    public virtual void OnTickStart() { }
    public virtual void OnTickEnd() { }
    public virtual void OnPlayerRespawn(Player player) { }
}
