using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Destination : TileNode
{
    private GameObject LevelCamera;
    public static int FinalLevelIndex = 4;
    private string nextLevel;
    private Animator animator;
    private bool isInAnimation;
    public GameObject startPoint;
    public float camSwitchBeforeDelay;
    public AudioClip teleport;

    void Start()
    {
        LevelCamera= GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
    }

    public override void OnPlayerEnter(Player player)
    {
        if (player.TryUseKey())
        {
            AudioManager.instance.PlaySingle(teleport);
            animator.Play("exit");
            isInAnimation = true;
            //judge if win
            if(CameraManager.level_index >= FinalLevelIndex)
            {
                player.GameWin();
                return;
            }
            Vector3 nextCheckPoint = new Vector3(startPoint.transform.position.x,
                startPoint.transform.position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
            player.Respawn(false);
        }
    }

    public override void OnPlayerRespawnStart(Player player)
    {
        base.OnPlayerRespawnStart(player);
        if (isInAnimation)
        {
            CameraManager.instance.SwitchLevelCamera();
        }
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        isInAnimation = false;
        
    }

}
