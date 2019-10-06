using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickups : MonoBehaviour
{
    public enum pickup_Type { Key, AP_supply };
    public int AP_supply_qty;
    public pickup_Type pickup;
    //public Doors pairedDoor;
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
        if (collision.gameObject.GetComponent<Player>() && pickup == pickup_Type.Key)
        {
            collision.gameObject.GetComponent<Player>().AddKey(this.gameObject);
            this.gameObject.SetActive(false);
        }
        if (collision.gameObject.GetComponent<Player>() && pickup == pickup_Type.AP_supply)
        {
            collision.gameObject.GetComponent<Player>().AddStep(AP_supply_qty);
            this.gameObject.SetActive(false);
        }

        //Debug.Log("Pick up key!");
        //Destroy(this.gameObject);

    }
}