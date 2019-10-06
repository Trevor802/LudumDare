using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseHandler : MonoBehaviour
{
    public GameObject corpse;
    private GameObject playerObj = null;
    // Start is called before the first frame update
    void Start()
    {
        if(playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCorpse();
        }
      
    }
    void SpawnCorpse()
    {
        Vector2 corpsePosition = playerObj.transform.position;
        Instantiate(corpse, corpsePosition,Quaternion.identity);
    }
}
