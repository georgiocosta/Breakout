using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed;

    public Vector3 moveDirection;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
       moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

       rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
    }
}
