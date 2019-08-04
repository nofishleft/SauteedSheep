using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D body;
    public SpriteRenderer sRenderer;

    public float health = 100f;
    public LayerMask terrainMask;
    public LayerMask bulletMask;

    public int bulletLayer;
    public int enemyLayer;

    public float SpeedMultiplier = 1f;
    public float DashMax = 10f;
    public float CurrentDash = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SoundMonitor());

        if (animator == null) animator = GetComponent<Animator>() ?? gameObject.AddComponent<Animator>();
        if (body == null) body = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
        if (sRenderer == null) sRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) return;

        CurrentDash = Mathf.Clamp(CurrentDash += Time.deltaTime, 0f, DashMax);

        Move();
        if (Input.GetKeyDown(KeyCode.LeftShift) && CurrentDash == DashMax)
        {
            CurrentDash = 0f;
            StartCoroutine(Dash((sRenderer.flipX) ? -1f : 1f));
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && LandAnimFin)
        {
            Jump();
        }
        else
        {
            StateUpdater();
        }
    }

    void FixedUpdate()
    {
        Vector2 v = body.velocity;
        v.x = speed * SpeedMultiplier;
        if (Dashing)
        {
            v.y = 0;
            body.gravityScale = 0f;
        }
        else
        {
            body.gravityScale = 1f;
        }
        body.velocity = v;
    }

    public bool Grounded
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, terrainMask);

            if (hit.collider != null)
            {
                return true;
            }
            return false;
        }
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
        if (!Dashing)
            speed = Input.GetAxis("Horizontal");

        if (speed < 0)
        {
            sRenderer.flipX = true;
        }
        else if (speed > 0)
        {
            sRenderer.flipX = false;
        }
    }

    private void Jump()
    {
        src.Pause();
        Vector2 v = body.velocity;
        v.y = 5;
        body.velocity = v;

        LandAnimFin = false;

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

        if (!IsGrounded)
        {
            //animator.SetBool("Jump", true);

            if (Falling)
            {
                animator.SetBool("Falling", true);
                PlayingJumpAnimation = true;
            }
        }
        else
        {
            if (PlayingJumpAnimation)
            {
                animator.SetBool("Landing", true);
                animator.SetBool("Falling", false);
                animator.SetBool("Jump", false);
                PlayingJumpAnimation = false;
            }
        }
    }

    bool IsGrounded = true;

    public int touchDamageLayer;
    void OnCollisionEnter2D(Collision2D col)
    {
        if ((1 << col.gameObject.layer & terrainMask) != 0)
        {

            for (int i = 0; i < col.contactCount; ++i)
            {
                ContactPoint2D point = col.GetContact(i);
                if (point.point.y <= transform.position.y)
                {
                    animator.SetBool("Landing", false);
                    animator.SetBool("Falling", false);
                    animator.SetBool("Jump", false);

                    if (!Dashing)
                        animator.Play("Landing");

                    IsGrounded = true;

                    break;
                }
            }
        }
        else if ((1 << col.gameObject.layer & bulletMask) != 0)
        {
            Debug.Log("Hit by bullet");
            Damage(10f);
        }
        else if (col.gameObject.layer == enemyLayer)
        {
            Debug.Log("Hit by enemy");
            Damage(10f);
        } else if (col.gameObject.layer == touchDamageLayer)
        {
            Damage(10f);
        }
    }

    void OnParticleCollision(GameObject o)
    {
        Debug.Log("Hit by bullet");
        Damage(10f);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer == enemyLayer)
        {
            Damage(Time.fixedDeltaTime * 10f);
            return;
        }

        IsGrounded = true;

        animator.SetBool("Landing", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Jump", false);

        if (animator.GetBool("Landing"))
        {

        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        for (int i = 0; i < col.contactCount; ++i)
        {
            ContactPoint2D point = col.GetContact(i);
            if (point.point.y <= transform.position.y)
            {
                animator.SetBool("Jump", true);

                if (!Dashing)
                    animator.Play("StartJump");

                IsGrounded = false;

                break;
            }
        }
    }

    private bool LandAnimFin = true;
    public void LandAnimFinish()
    {
        LandAnimFin = true;
    }

    public bool Dashing;
    public bool PostDash;

    IEnumerator Dash(float direction)
    {
        src.Pause();
        Dashing = true;
        animator.SetBool("Dashing", true);
        float t = 0.5f;
        do {
            yield return null;
            speed = direction * 5f;
        } while ((t -= Time.deltaTime) > 0);

        Dashing = false;
        PostDash = true;
        yield return null;
        if (body.velocity.y < 0) animator.SetBool("Falling", true);
        
        animator.SetBool("Dashing", false);
    }

    public void Damage(float dmg)
    {
        if (Dashing) return;

        health = Mathf.Clamp(health - dmg, 0, 100f);
        animator.SetTrigger("Hurt");

        if (health == 0)
        {
            src.Stop();
            animator.SetBool("Dead", true);
        }
    }

    public AudioSource src;
    public AudioClip walkingNoiseLoop;
    public Settings settings;

    IEnumerator SoundMonitor()
    {
        src.clip = walkingNoiseLoop;
        src.loop = true;

        float f = 1f;

        src.Play();
        src.Pause();

        while (health > 0)
        {
            src.volume = settings.GetGameVolume();
            yield return new WaitForSeconds(f);
            if (Grounded && speed != 0 && !Dashing)
            {
                if (!src.isPlaying) src.UnPause();
                f = 0.5f;
                
            }
            else
            {
                f = 0.1f;
                src.Pause();
            }
        }

        src.Stop();
    }
}
