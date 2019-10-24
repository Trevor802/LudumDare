using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickups : TileNode
{
    public enum pickup_Type { Key, AP_supply };
    public int AP_supply_qty;
    public pickup_Type pickup;
    public AudioClip pickUp;
    private AudioSource source;
    //public Doors pairedDoor;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO Use Tick
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && pickup == pickup_Type.Key)
        {
            AudioManager.instance.PlaySingle(pickUp);
            StartCoroutine(DeactivateUpItem(collision));
        }
        if (collision.gameObject.GetComponent<Player>() && pickup == pickup_Type.AP_supply)
        {
            collision.gameObject.GetComponent<Player>().AddStep(AP_supply_qty);
            this.gameObject.SetActive(false);
        }

        //Debug.Log("Pick up key!");
        //Destroy(this.gameObject);

    }

    private IEnumerator DeactivateUpItem(Collider2D collision)
    {
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
        collision.gameObject.GetComponent<Player>().AddKey(this.gameObject);
        //
    }
}