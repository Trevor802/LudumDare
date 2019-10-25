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
        ENDING = 3
    }
}

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Initialization
        lives = initLives;
        steps = initSteps;
        inverseMoveTime = 1f / moveTime;
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        headKey = this.transform.Find("HeadKey").gameObject;
        keySprite = headKey.GetComponent<SpriteRenderer>();
        keySprite.sortingOrder = 1;
    }
    #endregion

    #region PlayerData
    [Header("Audio Clips")]
    public AudioClip footStep;
    public AudioClip death;
    public AudioClip respawn;
    [Header("Configuration")]
    public int numLevels = 5;
    public float moveTime = 0.5f;
    public int initLives = 7;
    public int initSteps = 7;
    public float deathAnimDur = 0.5f;
    public float camSwitchDur = 1f;
    public float respawnAnimDur = 0.5f;
    public LayerMask blockingLayer;
    [Header("Game Objects")]
    public GameObject keyInstance;
    public SpriteRenderer[] stepIcons;
    #endregion PlayerData

    private Vector3 respawnPos;
    private int steps;
    private int lives;
    private bool hasKey = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Vector2 lastMove;
    private float inverseMoveTime;
    private Coroutine movingCoroutine;
    private GameObject headKey;
    private Animator animator;
    private SpriteRenderer keySprite;
    private VII.PlayerState playerState;
    private int currentLevel;

    private void Start()
    {
        UpdateStepUI();
        UIManager.Instance.InitUI();
        UIManager.Instance.UpdateUI();
    }

    public bool Move(int xDir, int yDir, out RaycastHit2D hit,
        bool costStep = true, bool smoothMove = true)
    {
        AudioManager.Instance.PlaySingle(footStep);
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
                VII.VIIEvents.TickStart.Invoke();
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
        VII.VIIEvents.TickEnd.Invoke();
        UpdateStepUI();
        if (steps <= 0 && playerState != VII.PlayerState.ENDING)
        {
            AudioManager.Instance.PlaySingle(death);
            Respawn();
        }
        // UI UPDATE
        UIManager.Instance.UpdateUI();
    }

    private IEnumerator Respawning(bool costLife)
    {
        float spawnDur = deathAnimDur;
        if (costLife)
        {
            spawnDur += camSwitchDur;
            animator.Play("Death");
        }

        HideStepIcon();
        // yield return new WaitForSeconds(spawnDur);

        // EVENT: Respawing Ends
        Vector3 deathPos = transform.position;
        Quaternion deathRot = transform.rotation;
        ObjectPooler.Instance.SpawnFromPool("Body", deathPos, deathRot);
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
        AudioManager.Instance.PlaySingle(respawn);
        yield return new WaitForSeconds(respawnAnimDur);
        // Respawning Ends
        playerState = VII.PlayerState.IDLE;
        // Broadcast with Event System
        VII.VIIEvents.PlayerRespawnEnd.Invoke(this);
        animator.Play("WalkDown");
        keySprite.sortingOrder = 1;
        // UI UPDATE
        UpdateStepUI();
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
            if (playerState == VII.PlayerState.IDLE)
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
            VII.SceneManager.Instance.LoadScene(VII.SceneType.GameScene);
        }
    }

    public void ResetRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }

    public void Respawn(bool costLife = true)
    {
        // Respawn Start
        if (playerState == VII.PlayerState.RESPAWING)
        {
            return;
        }
        playerState = VII.PlayerState.RESPAWING;
        if (costLife)
        {
            lives--;
        }
        else
        {
            lives = initLives;
        }
        StopCoroutine(movingCoroutine);
        VII.VIIEvents.PlayerRespawnStart.Invoke(this);
        if (lives <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(Respawning(costLife));
    }

    public void GameOver()
    {
        if (playerState == VII.PlayerState.ENDING) return;
        playerState = VII.PlayerState.ENDING;
        UIManager.Instance.ClearUI();
        VII.SceneManager.Instance.LoadScene(VII.SceneType.RestartScene);
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
        playerState = VII.PlayerState.ENDING;
        UIManager.Instance.ClearUI();
        VII.SceneManager.Instance.LoadScene(VII.SceneType.WinScene);
    }

    public void AddLevel()
    {
        currentLevel++;
    }

    // Getter

    public VII.PlayerState GetPlayerState() { return playerState; }
    public int GetCurrentLevel() { return currentLevel; }
    public int GetSteps() { return steps; }
    public int GetLives() { return lives; }
    public bool GetHasKey() { return hasKey; }
    public Vector2 GetLastMove() { return lastMove; }
}
