using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSheep : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    public float frameInterval;

    public Sprite[] sprites;

    public DialogueSwitcher dialogue;

    void Start()
    {
        StartCoroutine(Animate());
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

    void OnTriggerEnter2D(Collider2D col)
    {
        dialogue.gameObject.SetActive(true);
        dialogue.Reset();
        Align(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Align(col);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        dialogue.Reset();
        dialogue.gameObject.SetActive(false);
    }

    void Align(Collider2D col)
    {
        if (col.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        } else if (col.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
}
