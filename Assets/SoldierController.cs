using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public string CurrentAnimation = "Idle";
    public string NextAnimation = "Idle";

    public const string _Aim = "Aim";
    public const string _Die = "Die";
    public const string _Grenade = "Grenade";
    public const string _Idle = "Idle";
    public const string _Jump = "Jump";
    public const string _Lower = "Lower";
    public const string _Run = "Run";
    public const string _Shoot = "Shoot";
    public const string _Walk = "Walk";

    public Animator animator;
    public SpriteRenderer sRenderer;
    public Rigidbody2D body;
    public ParticleSystem particle;

    public LayerMask enemyMask;
    public float viewRange;
    public float health = 100f;

    private Vector2 playerLastPos;
    private bool trackingPlayer;
    private bool canSeeEnemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateFunction());
    }

    IEnumerator UpdateFunction()
    {
        yield return true;
        while (true)
        {

            //float direction = sRenderer.flipX ? 1: -1;
            float direction = transform.localScale.x > 0 ? -1 : 1;

            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up * 0.5f, new Vector2(direction, 0), viewRange,enemyMask);
            canSeeEnemy = hit.collider != null;
            if (canSeeEnemy)
            {
                playerLastPos = hit.collider.transform.position;
                trackingPlayer = true;
            }
            //Debug.Log($"Current: {CurrentAnimation}, Next: {NextAnimation}");
            bool cont = false;

            switch (CurrentAnimation)
            {
                case _Aim:
                    if (canSeeEnemy) NextAnimation = _Shoot;
                    break;
                case _Die:
                    NextAnimation = "";
                    break;
                case _Grenade:
                    break;
                case _Idle:
                    if (canSeeEnemy)
                    {
                        CurrentAnimation = _Aim;
                        animator.Play(_Aim);
                        cont = true;
                    }
                    else
                    {
                        //If can walk forwards
                        NextAnimation = _Walk;
                        //Else stand for a bit then turn around
                    }
                    break;
                case _Jump:
                    break;
                case _Lower:
                    if (canSeeEnemy) NextAnimation = _Aim;
                    else
                    {
                        NextAnimation = _Run;
                        Vector2 v = playerLastPos - (Vector2)transform.position;
                        v.y = 0;
                        dir = (v.x < 0) ? -1 : (v.x > 0 ? 1 : 0);
                    }
                    break;
                case _Run:
                    if (canSeeEnemy)
                    {
                        CurrentAnimation = _Aim;
                        animator.SetTrigger(CurrentAnimation);
                    }
                    else if (trackingPlayer)
                    {

                    }
                    else
                    {
                        CurrentAnimation = _Idle;
                        animator.SetTrigger(CurrentAnimation);
                    }
                    break;
                case _Shoot:
                    if (canSeeEnemy) NextAnimation = _Shoot;
                    else NextAnimation = _Lower;
                    break;
                case _Walk:
                    if (canSeeEnemy) {
                        CurrentAnimation = _Aim;
                        animator.SetTrigger(CurrentAnimation);
                    }
                    break;
                default:
                    //Grenade, Run, Jump, Lower
                    break;
                    
            }

            if (cont) continue;

            if (health <= 0)
            {
                health = 0;
                NextAnimation = "";
                animator.SetTrigger(_Die);
            }

            yield return true;
        }
    }

    private float dir = 0;

    void FixedUpdate()
    {
        if (trackingPlayer && !canSeeEnemy && CurrentAnimation == _Run)
        {
            Vector2 v = playerLastPos - (Vector2)transform.position;
            v.y = 0;
            float ndir = (v.x < 0) ? -1 : (v.x > 0 ? 1 : 0);

            if (ndir != dir)
            {
                Vector2 v2 = body.velocity;
                v2.x = 0;
                body.velocity = v2;
                trackingPlayer = false;
                return;
            }

            v.x = 5f * dir;
            v.y = body.velocity.y;
            body.velocity = v;
        }
        else
        {
            Vector2 v2 = body.velocity;
            v2.x = 0;
            body.velocity = v2;
        }
    }

    public void OnShoot()
    {
        particle.Emit(1);
    }

    public void OnGrenade()
    {

    }

    public void OnAnimationFinished()
    {
        animator.SetTrigger(NextAnimation);
        CurrentAnimation = NextAnimation;
    }

    public void OnShootFinished()
    {

    }

    public void OnGrenadeFinished()
    {

    }
}