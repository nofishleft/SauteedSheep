using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Collider2D col;
    public float triggerDelay;
    public float timeBetweenFrames;
    public float cooldown;
    public int enableColiider;
    public int disableColiider;

    public Sprite normalSprite;
    public Sprite[] sprites;

    public Player player;

    public List<Collider2D> InAreaObjs = new List<Collider2D>();

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        InAreaObjs.Add(col);
        if (!folding) StartCoroutine(Fold());
        inArea = true;
        damaged = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!folding) StartCoroutine(Fold());
        inArea = true;
    }

    bool inArea = false;
    void OnTriggerExit2D(Collider2D col)
    {
        InAreaObjs.Remove(col);
        inArea = false;
        damaged = false;
    }

    bool folding = false;
    IEnumerator Fold()
    {
        folding = true;

        spriteRenderer.sprite = normalSprite;
        yield return new WaitForSeconds(triggerDelay);
        for (int i = 0; i < sprites.Length; ++i)
        {
            spriteRenderer.sprite = sprites[i];
            if (i >= enableColiider && i < disableColiider) Damage();
            else damaged = false;
            yield return new WaitForSeconds(timeBetweenFrames);
        }

        spriteRenderer.sprite = normalSprite;

        yield return new WaitForSeconds(cooldown);
        folding = false;
    }

    bool damaged = false;

    void Damage()
    {
        if (!damaged)
            foreach (Collider2D col in InAreaObjs)
            {
                SoldierController s = col.gameObject.GetComponent<SoldierController>();
                if (s != null)
                {
                    damaged = true;
                    s.animator.SetTrigger(SoldierController._Die);
                    s.CurrentAnimation = SoldierController._Die;
                    s.NextAnimation = SoldierController._Die;
                }
                else
                {
                    if (col.gameObject == player.gameObject)
                    {
                        damaged = true;
                        player.Damage(10f);
                    }
                }
            }
    }
}
