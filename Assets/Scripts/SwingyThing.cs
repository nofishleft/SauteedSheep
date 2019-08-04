using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingyThing : MonoBehaviour
{
    public float angMax;
    private float ang;
    public float tMax;
    private float t;
    private bool decreasing;
    public int playerLayer;
    public Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();

        //Randomize position and direction
        t = Random.Range(-tMax, tMax);
        decreasing = Random.Range(0, 2) == 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0, angMax * Mathf.Sign(t) * (0.1f*Mathf.Sqrt(Mathf.Abs(t)) + Mathf.Sin(Mathf.Abs(t))));
        if (decreasing)
        {
            t -= Time.deltaTime;
            if (t <= -tMax)
            {
                t = -tMax;
                decreasing = false;
            }
        }
        else
        {
            t += Time.deltaTime;
            if (t >= tMax)
            {
                t = tMax;
                decreasing = true;
            }
        }
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.layer == playerLayer || col.gameObject == player.gameObject)
        {
            player.Damage(10f);
        }
    }
}
