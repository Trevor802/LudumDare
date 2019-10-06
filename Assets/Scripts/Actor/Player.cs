using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveTime = 0.5f;
    public int lives = 3;
    public int initSteps = 7;
    public int steps;
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

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        initPos = transform.position;
        UIManager.instance.UpdateUI();
        inverseMoveTime = 1 / moveTime;
    }

    public bool Move(int xDir, int yDir, out RaycastHit2D hit,
        bool costStep = true)
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
                steps--;
                movingCoroutine = StartCoroutine(SmoothMovement(end));
                foreach (TileNode tile in FindObjectsOfType<TileNode>())
                {
                    tile.OnTickStart();
                }
            }
            return true;
        }
        return false;
    }

    public IEnumerator SmoothMovement(Vector3 end)
    {
        moving = true;
        // Set the player's z position to 0, or remove the z value while calculating the distance
        float sqrDistance = (transform.position - end).sqrMagnitude;
        while(sqrDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPos);
            sqrDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        moving = false;
        foreach (TileNode tile in FindObjectsOfType<TileNode>())
        {
            tile.OnTickEnd();
        }
        if (steps <= 0)
        {
            lives--;
            if (lives <= 0)
            {
                Respawn();
                GameOver();
            }
            else
            {
                Respawn();
            }
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
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawn(this);
        }
        Vector3 deathPos = transform.position;
        transform.position = initPos;
        steps = initSteps;
        lastMove = Vector2.zero;
        if (hasKey)
        {
            keyInstance.transform.position = deathPos;
            keyInstance.SetActive(true);
            hasKey = false;
        }
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
            return true;
        }
        return false;
    }

    public void AddKey(GameObject picked_key)
    {
        keyInstance = picked_key;
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
