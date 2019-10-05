using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    
    public Doors pairedDoor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Pick up key!");
        //Destroy(this.gameObject);
        pairedDoor.ToggleDoorState();
        this.gameObject.SetActive(false);
    }
}