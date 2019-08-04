using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float range;
    public LayerMask visionMask;
    public int playerLayer;

    public float frameInterval;

    public Sprite[] sprites;

    private bool looking = true;
    private Vector2 position;

    void Start()
    {
        StartCoroutine(Animate());
        StartCoroutine(Charge());
    }

    private (bool, Vector2) CastLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, visionMask);
        bool b = hit.collider != null && hit.collider.gameObject.layer == playerLayer;
        if (hit.collider != null) Debug.Log(hit.collider.gameObject.layer);
        return (b, b ? hit.point : Vector2.zero);
    }

    private (bool, Vector2) CastRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, visionMask);
        bool b = hit.collider != null && hit.collider.gameObject.layer == playerLayer;
        if (hit.collider != null) Debug.Log(hit.collider.gameObject.layer);
        return (b, b ? hit.point : Vector2.zero);
    }

    IEnumerator Animate()
    {
        while (true)
        {
            for (int i = 0; i < sprites.Length; ++i)
            {
                spriteRenderer.sprite = sprites[i];
                yield return new WaitForSeconds(frameInterval);
            }
        }
    }

    IEnumerator Charge()
    {
        yield return null;

        while (true)
        {
            (bool hit, Vector2 pos) = CastLeft();
            if (hit)
            {
                spriteRenderer.flipX = false;
                looking = false;
                pos.y = transform.position.y;
                position = pos;
                Move();
                yield return null;
                continue;
            }

            (hit, pos) = CastRight();
            if (hit)
            {
                spriteRenderer.flipX = true;
                looking = false;
                pos.y = transform.position.y;
                position = pos;
                Move();
                yield return null;
                continue;
            }

            if (looking == false)
            {
                Move();
            }

            yield return null;
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, position, 5f * Time.deltaTime);
        if (transform.position.x == position.x)
        {
            looking = true;
        }
    }
}
