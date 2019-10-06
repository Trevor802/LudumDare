using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerboard : MonoBehaviour
{
    bool Triggered = false;
    bool PlayerDiedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTick()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>())
        {
            Triggered = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>() && Player.dying == true)
        {
            PlayerDiedOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (PlayerDiedOn == false)
        {
            Triggered = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Triggered == true)
        {
            GameObject door = GameObject.Find("door");
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (Triggered == false)
        {
            GameObject door = GameObject.Find("door");
            door.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
