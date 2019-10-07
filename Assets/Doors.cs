using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Doors : TileNode
{
    private GameObject LevelCamera;
    public int levelIndex;
    private string nextLevel;
    private Animator animator;
    private bool isInAnimation;
    public GameObject startPoint;
<<<<<<< Updated upstream
    public float camSwitchBeforeDelay;
=======
    private AudioSource source;
>>>>>>> Stashed changes
    void Start()
    {
        LevelCamera= GameObject.FindGameObjectWithTag("MainCamera");
        GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public override void OnPlayerEnter(Player player)
    {
        if (player.TryUseKey())
        {
            source.Play();
            animator.Play("exit");
            isInAnimation = true;
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
            LevelCamera.GetComponent<CameraManager>().SwitchLevelCamera();
            levelIndex = LevelCamera.GetComponent<CameraManager>().level_index;
            Vector3 nextCheckPoint = new Vector3(startPoint.transform.position.x,
                startPoint.transform.position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
        }
    }

    public override void OnPlayerRespawnEnd(Player player)
    {
        base.OnPlayerRespawnEnd(player);
        
        
    }

}
