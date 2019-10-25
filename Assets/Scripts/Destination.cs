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
    public int cnt = 0;

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
            AudioManager.instance.PlaySingle(teleport);
            animator.Play("exit");
            isInAnimation = true;
            //judge if win
            if(CameraManager.level_index >= FinalLevelIndex)
            {
                player.GameWin();
                return;
            }
            player.Respawn(false);
        }
    }

    public override void OnPlayerRespawnStart(Player player)
    {
        base.OnPlayerRespawnStart(player);
        StartCoroutine(Delay(player));
    }

    private IEnumerator Delay(Player player)
    {
        yield return new WaitForSeconds(camSwitchBeforeDelay);
        if (isInAnimation)
        {
            StopAllCoroutines();
            LevelCamera.GetComponent<CameraManager>().SwitchLevelCamera();
            Vector3 nextCheckPoint = new Vector3(startPoint.transform.position.x,
                startPoint.transform.position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
        }
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        isInAnimation = false;
        
    }

}
