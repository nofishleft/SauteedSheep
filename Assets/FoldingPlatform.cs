using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingPlatform : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Collider2D col;
    public float timeToFirst;
    public float timeToFold;
    public float timeToRestorePartial;
    public float timeToRestoreFull;

    public Sprite NormalSprite;
    public Sprite FoldingSprite;
    public Sprite FoldedSprite;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!folding) StartCoroutine(Fold());
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!folding) StartCoroutine(Fold());
    }

    bool folding = false;
    IEnumerator Fold()
    {
        spriteRenderer.sprite = NormalSprite;
        folding = true;
        yield return new WaitForSeconds(timeToFirst);
        spriteRenderer.sprite = FoldingSprite;
        yield return new WaitForSeconds(timeToFold);
        spriteRenderer.sprite = FoldedSprite;
        col.enabled = false;
        yield return new WaitForSeconds(timeToRestorePartial);
        spriteRenderer.sprite = FoldingSprite;
        yield return new WaitForSeconds(timeToRestoreFull);
        spriteRenderer.sprite = NormalSprite;
        col.enabled = true;
        folding = false;
    }
}
