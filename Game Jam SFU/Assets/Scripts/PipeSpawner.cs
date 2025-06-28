using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float flapForce = 5f;

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

        // On spacebar press, apply upward force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }
    }

    void Flap()
    {
        // Reset vertical velocity before applying force for consistent flaps
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with pipes or ground
        isDead = true;
        rb.velocity = Vector2.zero;
        // Inform GameManager or play death animation here
        GameManager.Instance.OnPlayerDeath();
    }
}
