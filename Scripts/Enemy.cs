using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public float RunSpeed = 10f;

    public float RushDistance = 10.0f;

    [SerializeField]
    private bool _isMoving = false;

    public GameObject player;

    public float distance;

    public GameObject PointA;

    public GameObject PointB;

    public float smoothTime = 10.0f;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("IsMoving", value);
        }
    }

    public bool _isFacingRight = true;


    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            _isFacingRight = value;
        }
    }

    Rigidbody2D rb;

    Animator animator;

    public Transform CurrentPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentPos = PointB.transform;
        animator.SetBool("IsMoving", true);
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.LookAt(player.transform);
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < RushDistance)
        {
          transform.position = Vector2.SmoothDamp(transform.position, new Vector2(player.transform.position.x, transform.position.y), ref smoothVelocity, smoothTime);
       }
        */
        Vector2 point = CurrentPos.position - transform.position;
        if (CurrentPos == PointB.transform)
        {

            rb.velocity = new Vector2(RunSpeed, 0);
        }
        else
        {
            IsFacingRight = false;
            rb.velocity = new Vector2(-RunSpeed, 0);
        }

        if (Vector2.Distance(transform.position, CurrentPos.position) < 0.5f && CurrentPos == PointB.transform)
        {
            IsFacingRight = false;
            CurrentPos = PointA.transform;
        }

        if (Vector2.Distance(transform.position, CurrentPos.position) < 0.5f && CurrentPos == PointA.transform)
        {
            IsFacingRight = true;
            CurrentPos = PointB.transform;
        }
    }

    private void FixedUpdate()
    {
        if (IsFacingRight) { 
            rb.velocity = new Vector2(RunSpeed * Vector2.right.x, rb.velocity.y);
        } else
        { 
            rb.velocity = new Vector2(RunSpeed * Vector2.left.x, rb.velocity.y);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player") 
        {
            SceneManager.LoadScene("LoseScene");
        } else if (collision.gameObject.tag == "Wall") {

            IsFacingRight = !IsFacingRight;
        }
    }

    private void SetFaceDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;

        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
}
