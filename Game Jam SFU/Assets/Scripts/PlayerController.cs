// Assets/Scripts/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float flapForce = 5f;

    Rigidbody2D rb;
    bool shouldFlap = false;
    bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space))
            shouldFlap = true;
    }

    void FixedUpdate()
    {
        if (!shouldFlap) return;
        shouldFlap = false;

        // reset y-velocity so each flap feels consistent
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        // assume your pipes are tagged "Obstacle" and ground is tagged "Ground"
        if (collision.gameObject.CompareTag("Obstacle") ||
            collision.gameObject.CompareTag("Ground"))
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            GameManager.Instance.OnPlayerDeath();
        }
    }
}
