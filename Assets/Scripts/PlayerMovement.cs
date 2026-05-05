using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private GravityShift gravityShift;

    // Minimum dot between contact normal and "up" (adjusts for inverted gravity)
    private const float GroundDotThreshold = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityShift = GetComponent<GravityShift>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Jump opposite to gravity direction
            float jumpDirection = Mathf.Sign(rb.gravityScale);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpDirection);
        }
    }

    private bool IsGroundContact(Collision2D collision)
    {
        // Treat explicit tags as ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("NoFlip"))
            return true;

        // Otherwise inspect contact normals relative to gravity direction
        float gravitySign = Mathf.Sign(rb.gravityScale);
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y * gravitySign > GroundDotThreshold)
                return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGroundContact(collision))
        {
            isGrounded = true;

            // If this contact is with a NoFlip platform, disable gravity shift
            if (collision.gameObject.CompareTag("NoFlip"))
            {
                gravityShift?.SetCanFlip(false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsGroundContact(collision))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If leaving a NoFlip platform, re-enable gravity shift
        if (collision.gameObject.CompareTag("NoFlip"))
        {
            gravityShift?.SetCanFlip(true);
        }

        // If leaving any ground-like collider, clear grounded state.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("NoFlip"))
        {
            isGrounded = false;
            return;
        }

        isGrounded = false;
    }
}