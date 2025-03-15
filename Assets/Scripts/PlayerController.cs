using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jumping Params")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float fallMultiplier;
    private Vector2 gravityVector;

    [Header("Ground Check Params")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayerJumpButton.OnJumpButtonPressed += Jump;
    }

    private void OnDisable()
    {
        PlayerJumpButton.OnJumpButtonPressed -= Jump;
    }

    private void Start()
    {
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
    }

    private void Jump()
    {
        if (GroundCheck())
        {
            SoundManager.instance.PlaySfxSound(jumpSound, false, true);
            rb.AddForce(new Vector2(rb.velocity.x, jumpHeight), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity -= gravityVector * fallMultiplier * Time.deltaTime;
        }
    }

    private bool GroundCheck()
    {
        bool isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }

    public void LockYPosition()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void UnlockYPosition()
    {
        rb.isKinematic = false;
    }
}
