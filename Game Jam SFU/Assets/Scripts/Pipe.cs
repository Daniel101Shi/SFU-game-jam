// Assets/Scripts/Pipe.cs
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 2f;
    public float despawnX = -12f;

    Transform player;
    bool hasPassedPlayer = false;

    void Awake()
    {
        // find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // move left
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // optional: notify GameManager when the pair is passed
        if (!hasPassedPlayer && transform.position.x < player.position.x)
        {
            hasPassedPlayer = true;
            GameManager.Instance.OnPipePassed();
        }

        // deactivate when off-screen
        if (transform.position.x < despawnX)
            gameObject.SetActive(false);
    }

    // reset flag when reused by pool
    void OnEnable()
    {
        hasPassedPlayer = false;
    }
}
