using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 6f;
    public float flipHorizontalBoost = 10f;
    public float groundDist = 0.1f;

    [Header("Squash and Stretch")]
    public float squashAmount = 0.7f;
    public float stretchAmount = 1.3f;
    public float effectDuration = 0.1f;
    private Vector3 originalScale;
    private bool wasGroundedLastFrame;

    [Header("References")]
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator animator;

    private bool isGrounded;
    private bool canFlip;
    private bool isFlipping;

    // Cached input
    private float x;
    private float z;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        originalScale = transform.localScale;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // INPUT
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // GROUND CHECK
        float rayLength = 1.2f + groundDist;
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f,
                                     Vector3.down,
                                     rayLength,
                                     terrainLayer);

        // FALLING & LANDING
        if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            animator.SetBool("isFalling", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("isFalling", false);
            canFlip = true;
            isFlipping = false;

            if (!wasGroundedLastFrame)
            {
                StopAllCoroutines();
                StartCoroutine(ApplySquash());
            }
        }
        wasGroundedLastFrame = isGrounded;

        // JUMP & FLIP
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                animator.SetTrigger("jumpTrigger");
                canFlip = true;

                StartCoroutine(ApplyStretch());
            }
            else if (canFlip)
            {
                Debug.Log("FLIP TRIGGERED");

                canFlip = false;
                isFlipping = true;

                animator.SetTrigger("flipTrigger");

                float moveDir = x != 0 ? x : (sr.flipX ? -1 : 1);
                rb.linearVelocity = new Vector3(
                    moveDir * flipHorizontalBoost,
                    jumpForce * 0.6f,
                    z * flipHorizontalBoost
                );

                StartCoroutine(ApplyStretch());
                StartCoroutine(EndFlip());
            }
        }

        // ANIMATION & SPRITE FLIP
        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(x), Mathf.Abs(z)));

        if (x < 0) sr.flipX = true;
        else if (x > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        // NORMAL MOVEMENT (disabled during flip)
        if (!isFlipping)
        {
            rb.linearVelocity = new Vector3(x * speed, rb.linearVelocity.y, z * speed);
        }
    }

    IEnumerator EndFlip()
    {
        yield return new WaitForSeconds(0.4f); // match flip animation length
        isFlipping = false;
    }

    IEnumerator ApplySquash()
    {
        transform.localScale = new Vector3(
            originalScale.x * stretchAmount,
            originalScale.y * squashAmount,
            originalScale.z
        );

        yield return new WaitForSeconds(effectDuration);

        float elapsed = 0;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, elapsed / 0.1f);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    IEnumerator ApplyStretch()
    {
        transform.localScale = new Vector3(
            originalScale.x * squashAmount,
            originalScale.y * stretchAmount,
            originalScale.z
        );

        yield return new WaitForSeconds(effectDuration);

        float elapsed = 0;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, elapsed / 0.1f);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}