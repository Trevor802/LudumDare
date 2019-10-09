using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
    private AudioSource source;
    private ObjectPooler Pools;
    private bool respawning;
    private bool winning = false;


    //private UnityEvent playerRespawnStartEvent;
    //private UnityEvent playerRespawnEndEvent;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        if (UIManager.instance.gameOver)
        {
            transform.position = UIManager.instance.restartPos;
        }
        inverseMoveTime = 1 / moveTime;
        headKey = this.transform.Find("HeadKey").gameObject;
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        UpdateStepUI();
        keySprite = headKey.GetComponent<SpriteRenderer>();
        keySprite.sortingOrder = 1;
        Pools = GameObject.Find("Pools").GetComponent<ObjectPooler>();
        UIManager.instance.initUI();
    }

    public bool Move(int xDir, int yDir, out RaycastHit2D hit,
        bool costStep = true, bool smoothMove = true)
    {
        source.PlayOneShot(footStep);
        Vector2 start = transform.position;
        lastMove = new Vector2(xDir, yDir);
        Vector2 end = start + lastMove;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
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
            }
            return true;
        }
        return false;
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        // Move End
        moving = true;
        // Set the player's z position to 0, or remove the z value while calculating the distance
        float sqrDistance = (new Vector2(transform.position.x ,transform.position.y) - new Vector2(end.x, end.y)).sqrMagnitude;
        while(sqrDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPos);
            sqrDistance = (new Vector2(transform.position.x, transform.position.y) - new Vector2(end.x, end.y)).sqrMagnitude;
            yield return null;
        }
        moving = false;
        foreach (TileNode tile in FindObjectsOfType<TileNode>())
        {
            tile.OnTickEnd();
        }
        // UI UPDATE
        UIManager.instance.UpdateUI();
        UpdateStepUI();
        if (steps <= 0 && !winning)
        {
            source.PlayOneShot(death,0.25f);
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
        source.PlayOneShot(respawn, 0.25f);
        yield return new WaitForSeconds(respawnAnimDur);
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
        if(horizontal != 0)
        {
            vertical = 0;
        }
        if(horizontal !=0 || vertical != 0)
        {
            if (!moving)
            {
                if (horizontal != 0)
                {
                    if (horizontal == 1)
                    {
                        animator.Play("WalkRight");
                        keySprite.sortingOrder = 1;
                    }
                    else
                    {
                        animator.Play("WalkLeft");
                        keySprite.sortingOrder = 1;
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
                        keySprite.sortingOrder = 1;
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
            SceneManager.LoadScene("2Dlevel");
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
        SceneManager.LoadScene("RestartScene");
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
        //Debug.Log(keyInstance.name);
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
        foreach(var icon in stepIcons)
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
        Debug.Log("win");
        UIManager.instance.ClearUI();
        SceneManager.LoadScene("WinScene");
    }

}
