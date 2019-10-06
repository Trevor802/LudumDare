using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float moveTime = 0.5f;
    public int lives = 3;
    public int initSteps = 7;
    public int steps;
    public float respawnDelaySeconds = 0.5f;
    private Vector3 initPos;
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
    private Animator animator;

    //private UnityEvent playerRespawnStartEvent;
    //private UnityEvent playerRespawnEndEvent;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        initPos = transform.position;
        UIManager.instance.UpdateUI();
        inverseMoveTime = 1 / moveTime;
        headKey = this.transform.Find("HeadKey").gameObject;
        animator = GetComponent<Animator>();
    }

    public bool Move(int xDir, int yDir, out RaycastHit2D hit,
        bool costStep = true, bool smoothMove = true)
    {
        Vector2 start = transform.position;
        lastMove = new Vector2(xDir, yDir);
        Vector2 end = start + lastMove;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
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
        if (steps <= 0)
        {
            Respawn();
        }
    }

    private IEnumerator Respawning()
    {
        moving = true;
        yield return new WaitForSeconds(respawnDelaySeconds);
        Vector3 deathPos = transform.position;
        transform.position = initPos;
        steps = initSteps;
        lastMove = Vector2.zero;
        moving = false;
        if (hasKey)
        {
            keyInstance.transform.position = deathPos;
            keyInstance.SetActive(true);
            headKey.SetActive(false);
            hasKey = false;
        }
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawnEnd(this);
        }
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
                    }
                    else
                    {
                        animator.Play("WalkLeft");
                    }
                }
                else
                {
                    if (vertical == 1)
                    {
                        animator.Play("WalkUp");
                    }
                    else
                    {
                        animator.Play("WalkDown");
                    }
                }
                RaycastHit2D hit;
                Move(horizontal, vertical, out hit);
                UIManager.instance.UpdateUI();
            }
        }
        horizontal = 0;
        vertical = 0;
    }

    public void ResetRespawnPos(Vector3 pos)
    {
        initPos = pos;
    }

    public void Respawn()
    {
        StopCoroutine(movingCoroutine);
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawnStart(this);
        }
        lives--;
        if (lives <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(Respawning());
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
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
        UIManager.instance.UpdateUI();
    }

    public void AddStep(int step)
    {
        steps += step;
        UIManager.instance.UpdateUI();
    }
}
