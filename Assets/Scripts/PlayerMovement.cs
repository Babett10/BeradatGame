using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7f; // Kecepatan karakter
    public float jumpForce = 12f; //Tinggi lompat
    private int maxJumps = 2; //max lompat
    private int _jumping;
    private float moveInput;
    private bool isAlive = true;
    private bool isPaused, infoActive, isWin = false;

    public GameObject QuizPanel;
    public Button buttonJump;
    public AudioSource source;
    public AudioClip jumpClip, startClip;

    private enum MovementState { idle, running, jumping, falling, doubleJump }

    [SerializeField] private LayerMask JumpableGround;

    private Animator anim;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _jumping = maxJumps;
        startSound();
    }

    void Update()
    {
        if (!isAlive || isPaused || infoActive || isWin || IsQuizPanelActive()) return;

        if (IsGrounded() && rb.velocity.y <= 0)
        {
            _jumping = maxJumps;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("Lompat = " + _jumping);
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        UpdateAnimationState();
    }

    public void SetMoveInput(float input)
    {

        moveInput = input;
    }

    public void Jump()
    {
        if (_jumping > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _jumping -= 1;
            Debug.Log("Lompat = " + _jumping);
            jumpSound();
        }
    }

    public void jumpSound()
    {
        source.PlayOneShot(jumpClip);
    }

    public void startSound()
    {
        source.PlayOneShot(startClip);
    }

    public void setAliveStatus(bool status)
    {
        isAlive = status;
    }

    public void setPausedStatus(bool status)
    {
        isPaused = status;
    }

    public void infoPanelActive(bool status)
    {
        infoActive = status;
    }

    public void setWinStatus(bool status)
    {
        isWin = status;
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (moveInput > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false; //hadap kanan
        }
        else if (moveInput < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true; //hadap kiri
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && _jumping == 1)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && _jumping == 0)
        {
            state = MovementState.doubleJump;
        }

        if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);
    }

    private bool IsQuizPanelActive()
    {
        for (int i = 0; i < QuizPanel.transform.childCount; i++)
        {
            if (QuizPanel.transform.GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
