using UnityEngine;

public class GravityFlip : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isUpsideDown = false;
    public float flipCooldown = 0.5f;
    private float lastFlipTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Time.time > lastFlipTime + flipCooldown)
        {
            FlipGravity();
            lastFlipTime = Time.time;
        }
    }

    void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;

        // Flip gravity
        rb.gravityScale *= -1;

        // Flip player visually
        transform.Rotate(0f, 0f, 180f);
    }
}
