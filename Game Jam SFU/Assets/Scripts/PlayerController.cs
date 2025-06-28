using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float flapForce = 5f;

    private Rigidbody2D rb;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        // Optional: Zero out velocity at start
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (isDead) return;

        // Capture spacebar press input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldFlap = true;
        }
    }

    void FixedUpdate()
    {
        if (shouldFlap)
        {
            // Reset vertical velocity before applying force for consistent flaps
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
            shouldFlap = false; // Reset flag after processing
        }
    }

    void OnCollisionEnter2D(Collision2D _)
    {
        // Check collision tags to determine if player should die
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            // Inform GameManager or play death animation here
            GameManager.Instance.OnPlayerDeath();
        }
    }
}
