using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : TileNode
{
    private GameObject camera;

    void Start()
    {
        camera= GameObject.FindGameObjectWithTag("MainCamera");
        GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            if (collision.gameObject.GetComponent<Player>().TryUseKey())
                camera.GetComponent<CameraManager>().SwitchLevelCamera();
                this.gameObject.SetActive(false);
        }
    }
}
