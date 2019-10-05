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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (openable)
            this.gameObject.SetActive(false);
    }
}
