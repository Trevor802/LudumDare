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

    // Update is called once per frame
    void Update()
    {
        if (Triggered == true)
        {
            GameObject door = GameObject.Find("door");
            door.transform.position = new Vector3(-6, 4, 0);
        }
    }
    public void OnTick()
    {

    }

    void OnTriggerEnter2D(Collision2D col)
    {
        Triggered = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (Player.dying == true)
        {
            PlayerDiedOn = true;
        }
    }

    void OnTriggerExit2D(Collision2D col)
    {
        if (PlayerDiedOn == false)
        {
            Triggered = false;
        }
    }
}
