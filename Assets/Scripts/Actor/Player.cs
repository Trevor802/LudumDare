using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lives = 3;
    public int initSteps = 7;
    public GameObject keyPrefab;
    private int steps;
    private Vector3 initPos;
    private bool hasKey = false;
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        initPos = transform.position;
    }

    bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            SmoothMovement(end);
            return true;
        }
        return false;
    }

    private void SmoothMovement(Vector3 end)
    {
        transform.position = end;
        steps--;
        if (steps <= 0)
        {
            Respawn();
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
            RaycastHit2D hit;
            Move(horizontal, vertical, out hit);
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
        Vector3 deathPos = transform.position;
        transform.position = initPos;
        if (hasKey)
        {
            Instantiate(keyPrefab, deathPos, Quaternion.identity);
        }
        steps = initSteps;
        hasKey = false;
        lives--;
        if (lives <=0)
        {
            GameOver();
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

    public void AddKey()
    {
        hasKey = true;
    }
}
