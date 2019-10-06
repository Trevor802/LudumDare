using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoints : MonoBehaviour
{
    private GameMaster gm;
    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.lastCheckPointPos = transform.position;
        }
    }
}
