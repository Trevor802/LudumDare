using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public static bool fall = false;
    bool filled = false;
    void LateUpdate()
    {
        fall = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            if(filled == true)
            {
                fall = false;
            }
            else
            {
                fall = true;
            }
            filled = true;
        }
    }
}
