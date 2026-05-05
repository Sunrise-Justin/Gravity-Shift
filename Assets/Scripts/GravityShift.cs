using UnityEngine;

public class GravityShift : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool canFlip = true;
    public float flipCooldown = 0.5f;
    private float lastFlipTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canFlip && Input.GetKeyDown(KeyCode.G) && Time.time > lastFlipTime + flipCooldown)
        {
            FlipGravity();
            lastFlipTime = Time.time;
        }
    }

    void FlipGravity()
    {
        // Invert gravity
        rb.gravityScale *= -1f;

        // Flip player visually
        transform.Rotate(0f, 0f, 180f);
    }

    // Called by PlayerMovement when landing on or leaving a NoFlip platform
    public void SetCanFlip(bool allowed)
    {
        canFlip = allowed;
    }
}