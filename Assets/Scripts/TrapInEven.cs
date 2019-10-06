using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInEven : TileNode
{
    public bool hitWithTrap = false;
    public int cnt = 0;
    public GameObject trap;
    // Start is called before the first frame update
    void Start()
    {

    }
    override public void OnTickStart()
    {
        cnt++;
        IsTrap();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void IsTrap()
    {
        if (cnt % 2 == 1)
        {

            GetComponent<SpriteRenderer>().enabled = false;
            // this.gameObject.SetActive(false);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && cnt % 2 == 0)
        {
            //hitWithTrap = true;
            collision.gameObject.GetComponent<Player>().Respawn();
            //this.gameObject.SetActive(false);
        }
    }
}
