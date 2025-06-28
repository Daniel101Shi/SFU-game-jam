// Assets/Scripts/Pipe.cs
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 2f;
    public float despawnX = -12f;

    private Transform player;
    private bool hasPassedPlayer = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        if (!hasPassedPlayer && transform.position.x < player.position.x)
        {
            hasPassedPlayer = true;
            GameManager.Instance.OnPipePassed();
        }

        if (transform.position.x < despawnX)
            gameObject.SetActive(false);
    }

    void OnEnable()
    {
        hasPassedPlayer = false;
    }
}
