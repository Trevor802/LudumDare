using UnityEngine;

public class Doors : TileNode
{
    private GameObject camera;

    void Start()
    {
        camera= GameObject.FindGameObjectWithTag("MainCamera");
        GameObject.FindGameObjectWithTag("MainCamera");
    }

    public override void OnPlayerEnter(Player player)
    {
        if (player.TryUseKey())
        {
            camera.GetComponent<CameraManager>().SwitchLevelCamera();
            this.gameObject.SetActive(false);
        }
    }
    
}
