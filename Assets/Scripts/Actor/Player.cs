using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveTime = 0.5f;
    public int initLives = 3;
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
    private GameObject step1;
    private GameObject step2;
    private GameObject step3;
    private GameObject step4;
    private GameObject step5;
    private GameObject step6;
    private GameObject step7;
    private Animator animator;
    private SpriteRenderer keySprite;

    //private UnityEvent playerRespawnStartEvent;
    //private UnityEvent playerRespawnEndEvent;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        steps = initSteps;
        initPos = transform.position;
        inverseMoveTime = 1 / moveTime;
        headKey = this.transform.Find("HeadKey").gameObject;
        step1 = this.transform.Find("step1").gameObject;
        step2 = this.transform.Find("step2").gameObject;
        step3 = this.transform.Find("step3").gameObject;
        step4 = this.transform.Find("step4").gameObject;
        step5 = this.transform.Find("step5").gameObject;
        step6 = this.transform.Find("step6").gameObject;
        step7 = this.transform.Find("step7").gameObject;
        animator = GetComponent<Animator>();
        UIManager.instance.UpdateUI();
        UpdateStepUI();
        keySprite = headKey.GetComponent<SpriteRenderer>();
        keySprite.sortingOrder = 1;
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
                UpdateStepUI();
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
        foreach (TileNode node in FindObjectsOfType<TileNode>())
        {
            node.OnPlayerRespawnEnd(this);
        }
        Vector3 deathPos = transform.position;
        transform.position = initPos;
        lastMove = Vector2.zero;
        moving = false;
        steps = initSteps;
        UIManager.instance.UpdateUI();
        UpdateStepUI();
        if (hasKey)
        {
            keyInstance.transform.position = deathPos;
            keyInstance.SetActive(true);
            headKey.SetActive(false);
            hasKey = false;
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
                        keySprite.sortingOrder = 0;
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

    public void Respawn(bool costLife = true)
    {
        if (costLife)
        {
            lives--;
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
        StartCoroutine(Respawning());
    }

    public void GameOver()
    {
        UIManager.instance.ClearUI();
        SceneManager.LoadScene(1);
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
        UpdateStepUI();
    }

    private void UpdateStepUI()
    {
        if (steps == 7)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (steps == 6)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = true;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (steps == 5)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = true;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (steps == 4)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = true;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (steps == 3)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = true;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (steps == 2)
        {
            step1.GetComponent<SpriteRenderer>().enabled = false;
            step2.GetComponent<SpriteRenderer>().enabled = true;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (steps == 1)
        {
            step1.GetComponent<SpriteRenderer>().enabled = true;
            step2.GetComponent<SpriteRenderer>().enabled = false;
            step3.GetComponent<SpriteRenderer>().enabled = false;
            step4.GetComponent<SpriteRenderer>().enabled = false;
            step5.GetComponent<SpriteRenderer>().enabled = false;
            step6.GetComponent<SpriteRenderer>().enabled = false;
            step7.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

}
