using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    public float speed = 8f;
    private bool IsLadder = false;
    public bool IsLaddered
    {
        get
        {
            return IsLadder;
        }
        private set
        {
            IsLadder = value;
            animator.SetBool("IsLaddered", value);
        }
    }

    private bool IsClimbing;

    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (IsLadder && Mathf.Abs(vertical) >= 0f)
        {
            IsClimbing = true;
        }

    }

    private void FixedUpdate()
    {
        if (IsClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        } else
        {
            rb.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Ladder"))
        {
            IsLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            IsLadder = false;
            IsClimbing = false;
        }
    }
}
