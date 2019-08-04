using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public float frameInterval;

    public Sprite[] sprites;


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

}
