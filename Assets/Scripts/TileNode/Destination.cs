using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Destination : TileNode
{
    private Animator animator;
    private bool isInAnimation;
    public GameObject startPoint;
    public float camSwitchBeforeDelay;
    public AudioClip teleport;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void OnPlayerEnter(Player player)
    {
        if (player.TryUseKey())
        {
            AudioManager.Instance.PlaySingle(teleport);
            animator.Play("exit");
            isInAnimation = true;
            Player.Instance.AddLevel();
            //judge if win
            if(Player.Instance.GetCurrentLevel() >= Player.Instance.numLevels)
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
            CameraManager.Instance.SwitchLevelCamera(1);
        }
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        isInAnimation = false;
        
    }

}
