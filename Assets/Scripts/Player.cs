using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace VII
{
    public enum PlayerState
    {
        IDLE = 0,
        MOVING = 1,
        RESPAWING = 2,
        WINNING = 3
    }
}

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioClip footStep;
    public AudioClip death;
    public AudioClip respawn;
    public float moveTime = 0.5f;
    public int initLives = 3;
    public int lives = 3;
    public int initSteps = 7;
    public int steps;
    public float deathAnimDur = 0.5f;
    public float camSwitchDur = 1f;
    public float respawnAnimDur = 0.5f;
    private Vector3 respawnPos;
    public bool hasKey = false;
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    public Vector2 lastMove;
    public GameObject keyInstance;
    private float inverseMoveTime;
    public bool moving;
    public Coroutine movingCoroutine;
    private GameObject headKey;
    public SpriteRenderer[] stepIcons;
    private Animator animator;
    private SpriteRenderer keySprite;
    private ObjectPooler Pools;
    private bool respawning;
    private bool winning = false;
    private VII.PlayerState playerState;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        if (UIManager.instance.gameOver)
        {
            transform.position = UIManager.instance.restartPos;
        }
        inverseMoveTime = 1f / moveTime;
        headKey = this.transform.Find("HeadKey").gameObject;
        animator = GetComponent<Animator>();
        UpdateStepUI();
        keySprite = headKey.GetComponent<SpriteRenderer>();
        keySprite.sortingOrder = 1;
        Pools = GameObject.Find("Pools").GetComponent<ObjectPooler>();
        UIManager.instance.initUI();
    }

    public bool Move(int xDir, int yDir, out RaycastHit2D hit,
        bool costStep = true, bool smoothMove = true)
    {
        AudioManager.instance.PlaySingle(footStep);
        Vector2 start = transform.position;
        lastMove = new Vector2(xDir, yDir);
        Vector2 end = start + lastMove;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            playerState = VII.PlayerState.MOVING;
            // Move Start
            if (costStep)
            {
                foreach (TileNode tile in FindObjectsOfType<TileNode>())
                {
                    tile.OnTickStart();
                }
                steps--;
            }
            if (smoothMove)
            {
                movingCoroutine = StartCoroutine(SmoothMovement(end));
            }
            else
            {
                transform.position = new Vector3(end.x, end.y, -1);
                playerState = VII.PlayerState.IDLE;
            }
            return true;
        }
        return false;
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        moving = true;
        // Set the player's z position to 0, or remove the z value while calculating the distance
        float sqrDistance = (new Vector2(transform.position.x, transform.position.y) - new Vector2(end.x, end.y)).sqrMagnitude;

        // Using while probably skips the Unity deltaTime duration
        while (sqrDistance > float.Epsilon)
        { 
            Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.fixedDeltaTime);
            rb2D.MovePosition(newPos);
            sqrDistance = (new Vector2(transform.position.x, transform.position.y) - new Vector2(end.x, end.y)).sqrMagnitude;
            yield return null;
        }
        // EVENT: Movement Ends
        playerState = VII.PlayerState.IDLE;
        moving = false;
        foreach (TileNode tile in FindObjectsOfType<TileNode>())
        {
            tile.OnTickEnd();
        }
        // UI UPDATE
        UIManager.instance.UpdateUI();
        UpdateStepUI();
        if (steps <= 0 && playerState != VII.PlayerState.WINNING)
        {
            AudioManager.instance.PlaySingle(death);
            Respawn();
        }
    }

    private IEnumerator Respawning(bool costLife)
    {
        // Respawn End

        moving = true;
        float spawnDur = deathAnimDur;
        if (costLife)
        {
            spawnDur += camSwitchDur;
            // Death Animation
            animator.Play("Death");
        }


        HideStepIcon();
        yield return new WaitForSeconds(spawnDur);
        Vector3 deathPos = transform.position;
        Quaternion deathRot = transform.rotation;
        Pools.SpawnFromPool("Body", deathPos, deathRot);
        if (hasKey)
        {
            keyInstance.transform.position = deathPos;
            keyInstance.SetActive(true);
            headKey.SetActive(false);
            hasKey = false;
        }
        transform.position = respawnPos;
        lastMove = Vector2.zero;
        steps = initSteps;
        // Respawn Animation
        animator.Play("Respawn");
        ShowStepIcon();
        AudioManager.instance.PlaySingle(respawn);
        yield return new WaitForSeconds(respawnAnimDur);
        // Respawning Ends
        playerState = VII.PlayerState.IDLE;
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawnEnd(this);
        }
        animator.Play("WalkDown");
        keySprite.sortingOrder = 1;
        moving = false;
        // UI UPDATE
        UpdateStepUI();
        respawning = false;
    }

    private void Update()
    {
        int horizontal = 0;
        int vertical = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontal = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontal = 1;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            vertical = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            vertical = -1;
        }
        if (horizontal != 0)
        {
            vertical = 0;
        }
        if (horizontal != 0 || vertical != 0)
        {
            if (!moving)
            {
                if (horizontal != 0)
                {
                    if (horizontal == 1)
                    {
                        animator.Play("WalkRight");
                        keySprite.sortingOrder = 10;
                    }
                    else
                    {
                        animator.Play("WalkLeft");
                        keySprite.sortingOrder = 10;
                    }
                }
                else
                {
                    if (vertical == 1)
                    {
                        //headKey.transform.localPosition = new Vector3(headKey.transform.position.x, headKey.transform.position.y, -2f);
                        keySprite.sortingOrder = -1;
                        animator.Play("WalkUp");
                    }
                    else
                    {
                        animator.Play("WalkDown");
                        keySprite.sortingOrder = 10;
                    }
                }
                RaycastHit2D hit;
                Move(horizontal, vertical, out hit);
            }
        }
        horizontal = 0;
        vertical = 0;

        if (Input.GetKeyDown(KeyCode.R))
        {
            VII.SceneManager.instance.LoadScene(VII.SceneType.GameScene);
        }
    }

    public void ResetRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }

    public void Respawn(bool costLife = true)
    {
        // Respawn Start
        if (respawning)
        {
            return;
        }
        respawning = true;
        playerState = VII.PlayerState.RESPAWING;
        if (costLife)
        {
            lives--;
            UIManager.instance.UpdateUI();
        }
        else
        {
            lives = initLives;
        }
        StopCoroutine(movingCoroutine);
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawnStart(this);
        }
        if (lives <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(Respawning(costLife));
    }

    public void GameOver()
    {
        if (winning) return;
        UIManager.instance.gameOver = true;
        UIManager.instance.levelIndex = CameraManager.level_index;
        UIManager.instance.restartPos = respawnPos;
        UIManager.instance.ClearUI();
        VII.SceneManager.instance.LoadScene(VII.SceneType.RestartScene);
    }

    public bool TryUseKey()
    {
        if (hasKey)
        {
            hasKey = false;
            headKey.SetActive(false);
            return true;
        }
        return false;
    }

    public void AddKey(GameObject picked_key)
    {
        keyInstance = picked_key;
        headKey.SetActive(true);
        hasKey = true;
    }

    public void AddStep(int step)
    {
        steps += step;
    }

    private void UpdateStepUI()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i == steps - 1)
            {
                stepIcons[i].enabled = true;
            }
            else
            {
                stepIcons[i].enabled = false;
            }

        }
    }

    private void HideStepIcon()
    {
        foreach (var icon in stepIcons)
        {
            icon.enabled = false;
        }
    }

    private void ShowStepIcon()
    {
        foreach (var icon in stepIcons)
        {
            icon.enabled = true;
        }
    }

    public void GameWin()
    {
        winning = true;
        playerState = VII.PlayerState.WINNING;
        UIManager.instance.ClearUI();
        VII.SceneManager.instance.LoadScene(VII.SceneType.WinScene);
    }

    // Getter

    public VII.PlayerState GetPlayerState() { return playerState; }

}
