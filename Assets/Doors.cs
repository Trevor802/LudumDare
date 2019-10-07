using UnityEngine;
using System.Collections.Generic;

public class Doors : TileNode
{
    private GameObject LevelCamera;
    public int levelIndex;
    private string nextLevel;
    private Animator animator;
    private bool isInAnimation;
    public GameObject startPoint;
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
            Vector3 nextCheckPoint = new Vector3(startPoint.transform.position.x,
                startPoint.transform.position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
        }
        
    }

}
