using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TileNode
{
   // public bool hitWithTrap = false;
    private int cnt = 0;
    //public GameObject trap;
    private int firstTrapAtStep = 0;//first trap appears in which step
    private int trapTimes = 2;// cnt%? 2 is odd/even
    public bool isTrapOnEven = true;
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
        if (cnt % trapTimes != 0)
        {
            if (isTrapOnEven == true)
            {
                isTrapOnEven = false;
            }
            else
            {
                isTrapOnEven = true;
            }
                
            GetComponent<SpriteRenderer>().enabled = isTrapOnEven;
                // this.gameObject.SetActive(false);
        }
        else
        {
            if(isTrapOnEven == false)
            {
                isTrapOnEven = true;
            }
            else
            {
                isTrapOnEven = false;
            }
            GetComponent<SpriteRenderer>().enabled = isTrapOnEven;
        }
        

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && isTrapOnEven == true)
        {
            //hitWithTrap = true;
            collision.gameObject.GetComponent<Player>().Respawn();
            //this.gameObject.SetActive(false);
        }
    }
}
