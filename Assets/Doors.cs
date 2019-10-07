using UnityEngine;
using System.Collections.Generic;

public class Doors : TileNode
{
    private GameObject LevelCamera;
    public int levelIndex;
    private string nextLevel;
    private Animator animator;
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
            LevelCamera.GetComponent<CameraManager>().SwitchLevelCamera();
            levelIndex = LevelCamera.GetComponent<CameraManager>().level_index;
            nextLevel = "Level" + (levelIndex + 1).ToString();
            Vector3 nextCheckPoint = new Vector3(GameObject.Find("Background").transform.Find("Grid").Find(nextLevel).Find("checkpoint").position.x, GameObject.Find("Background").transform.Find("Grid").Find(nextLevel).Find("checkpoint").position.y, player.transform.position.z);
            player.ResetRespawnPos(nextCheckPoint);
            player.Respawn(false);
            this.gameObject.SetActive(false);
        }
        
    }
    
}
