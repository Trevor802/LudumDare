using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VII
{
    public enum PickupType {
        KEY = 0,
        STEP = 1
    };
}

public class Pickups : TileNode
{
    
    public int numSteps;
    public VII.PickupType pickupType;
    public AudioClip pickUp;

    public override void OnPlayerEnter(Player player)
    {
        base.OnPlayerEnter(player);
        switch (pickupType)
        {
            case VII.PickupType.KEY:
                AudioManager.Instance.PlaySingle(pickUp);
                gameObject.SetActive(false);
                Player.Instance.AddKey(gameObject);
                break;
            case VII.PickupType.STEP:
                Player.Instance.AddStep(numSteps);
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}