using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided!!!");
    }
}
