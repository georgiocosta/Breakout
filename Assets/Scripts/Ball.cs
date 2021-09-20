using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public float speed;
    public int numberOfLives;

    public float breakCooldown, initialSpeed;

    private int hits, blockHits;

    private bool canBreak, canMove;

    private Vector2 direction, lastPosition;

    Rigidbody2D rb;
    AudioSource sfx;

    public Score score;
    public Lives lives;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();

        direction = StartDirection();

        hits = 0;
        blockHits = 0;

        canBreak = true;
        canMove = false;

        lives.SetLives(numberOfLives);
        Debug.Log(direction);

        initialSpeed = speed;
    }


    void FixedUpdate()
    {
        if (canMove)
        {
            Move();

            if (blockHits == 104)
            {
                SceneManager.LoadSceneAsync("Main");
            }

            if (numberOfLives < 0)
            {
                score.ResetPoints();
                SceneManager.LoadSceneAsync("Main");
            }
        }
        
        else if(!canMove && Input.GetAxis("Horizontal") != 0)
        {
            canMove = true;
        }
    }

    private void Move()
    {
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;

        sfx.Play();

        Vector2 collisionPoint = collision.GetContact(0).point;
        Vector2 collisionDirection = collisionPoint.normalized;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position - collisionDirection, collisionDirection);
        
        //Debug.Log(hit.normal);

        Vector2 normal = new Vector2();

        if (Mathf.Abs(hit.normal.x) > Mathf.Abs(hit.normal.y))
            normal = new Vector2(Mathf.Pow(hit.normal.x, 0), 0);
        else if (Mathf.Abs(hit.normal.y) > Mathf.Abs(hit.normal.x))
            normal = new Vector2(0, Mathf.Pow(hit.normal.y, 0));
        else
            Debug.Log("WEIRD NORMAL WEIRD NORMAL WEIRD NORMAL");

        Debug.Log(collision.GetContact(0).normal);

        direction = Vector2.Reflect(direction, collision.GetContact(0).normal);

        hits += 1;

        if(hits == 4 || hits == 12)
        {
            speed += 1;
        }

        if(collision.gameObject.GetComponent<Block>() && canBreak)
        {
            canBreak = false;
            StartCoroutine(CoolDown(breakCooldown));

            Block block = collision.gameObject.GetComponent<Block>();
            score.AddPoints(block.points);
            blockHits += 1;

            block.Break();
        }
        else if(collision.gameObject.CompareTag("Pit"))
        {
            numberOfLives--;
            lives.SetLives(numberOfLives);

            transform.position = Vector3.zero;
            hits = 0;
            speed = initialSpeed;
            canMove = false;
        }
        else if(collision.gameObject.GetComponent<Paddle>())
        {
            if(collision.gameObject.GetComponent<Paddle>().moveDirection.x != 0)
                direction = new Vector2(collision.gameObject.GetComponent<Paddle>().moveDirection.x, direction.y);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(rb.velocity.x == 0 && collision.gameObject.CompareTag("Wall"))
        {
            direction = new Vector2(Random.Range(0, 2) * 2 - 1, direction.y);
        }
    }

    private Vector2 StartDirection()
    {
        return new Vector3(Random.Range(0, 2) * 2 - 1, Random.Range(0, 2) * 2 - 1);
    }

    IEnumerator CoolDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        canBreak = true;
    }

    IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown)
            yield return null;
    }
}
