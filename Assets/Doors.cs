using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private bool openable;
    // Start is called before the first frame update
    void Start()
    {
        openable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoorState()
    {
        if (!openable)
            openable = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            if (collision.gameObject.GetComponent<Player>().TryUseKey())
                this.gameObject.SetActive(false);
        }
    }
}
