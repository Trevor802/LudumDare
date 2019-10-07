using UnityEngine;
using System.Collections.Generic;

public class Doors : TileNode
{
    private GameObject LevelCamera;
    public int levelIndex;
    private string nextLevel;
    private Animator animator;
    private bool isInAnimation;
    void Start()
    {
        LevelCamera= GameObject.FindGameObjectWithTag("MainCamera");
        GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
    }

    public override void OnPlayerEnter(Player player)
    {
        if (player.TryUseKey())
        {
            animator.Play("Trigger");
            isInAnimation = true;
            player.Respawn(false);
        }
        
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        if (isInAnimation)
        {
            LevelCamera.GetComponent<CameraManager>().SwitchLevelCamera();
            levelIndex = LevelCamera.GetComponent<CameraManager>().level_index;
            nextLevel = "Level" + (levelIndex + 1).ToString();
            GameObject obj = GameObject.Find("Background/Grid/" + nextLevel + "/checkpoint");
            Vector3 nextCheckPoint = new Vector3(GameObject.Find("Background/Grid/" + nextLevel + "/checkpoint").transform.position.x,
                GameObject.Find("Background/Grid/"+nextLevel+"/checkpoint").transform.position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
        }
        
    }

}
