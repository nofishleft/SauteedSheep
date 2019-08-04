using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D body;
    public CharacterController2D controller;

    public LayerMask terrainMask;
    public float SpeedMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>() ?? gameObject.AddComponent<Animator>();
        if (body == null) body = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        StateUpdater();
    }

    public bool Falling
    {
        get
        {
            if (body.velocity.y < 0)
            {
                return true;
            }
            return false;
        }
    }

    private bool PlayingJumpAnimation;

    public float speed;

    public void Move()
    {
        bool _jump = Input.GetKeyDown(KeyCode.Space) && !PlayingJumpAnimation;
        if (_jump) PlayingJumpAnimation = true;
        animator.SetBool("Jump", true);
        speed = Input.GetAxis("Horizontal");

        controller.Move(speed, false, _jump);
    }

    private void Jump()
    {
        if (!PlayingJumpAnimation)
        {
            PlayingJumpAnimation = true;
            animator.SetBool("Falling", false);
            animator.SetBool("Jump", true);
        }
    }

    private void StateUpdater()
    {
        animator.SetFloat("Speed", speed);

        if (speed != 0)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);

        if (controller.Grounded() && Mathf.Abs(body.velocity.y) < 0.01f)
        {
            //PlayingJumpAnimation = false;
            animator.SetBool("Falling", false);
            animator.SetBool("Jump", false);
        }
        else if (Falling)
        {
            animator.SetBool("Falling", true);
            PlayingJumpAnimation = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        for (int i = 0; i < col.contactCount; ++i)
        {
            ContactPoint2D point = col.GetContact(i);
            if (point.point.y <= transform.position.y)
            {
                animator.SetBool("Landing", false);
                animator.SetBool("Falling", false);
                animator.SetBool("Jump", false);

                animator.Play("Landing");

                break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        animator.SetBool("Landing", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Jump", false);

        if (animator.GetBool("Landing"))
        {

        }
    }

    public void OnLand()
    {
        Debug.Log("Landed");
        if (animator.GetBool("Falling") && body.velocity.y <= 0)
        {
            animator.SetBool("Landing", true);
            animator.SetBool("Falling", false);
            animator.SetBool("Jump", false);
        }
    }

    public void LandAnimFinish()
    {
        animator.SetBool("Landing", false);
        animator.SetBool("Falling", false);
        PlayingJumpAnimation = false;
    }
}
