using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("References")]
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator animator;

    // Cached input
    private float x;
    private float z;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Prevents the detective from tipping over when walking into walls
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. INPUT - Capture horizontal and depth movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 2. ANIMATION - Updates the "speed" parameter
        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Max(Mathf.Abs(x), Mathf.Abs(z)));
        }

        // 3. SPRITE FACING - Flips the sprite based on left/right movement
        if (x < 0) sr.flipX = true;
        else if (x > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        // 4. DIAGONAL FIX (Normalization)
        // We create a direction vector from our inputs
        Vector3 moveDir = new Vector3(x, 0, z);

        // If the vector length is greater than 1 (diagonal), we normalize it
        // This ensures diagonal speed is the same as straight-line speed
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        // 5. APPLY MOVEMENT
        // We keep rb.linearVelocity.y so gravity still pulls the player down
        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
    }
}