using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public float healAmount;
    public float bobAmount;
    public float bobSpeed;
    private float bob;
    private Vector3 v;
    private bool decreasing;
    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        v = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (decreasing)
        {
            bob -= Time.deltaTime * bobSpeed;
            if (bob <= 0)
            {
                bob = 0;
                decreasing = false;
            }
        }
        else
        {
            bob += Time.deltaTime * bobSpeed;
            if (bob >= bobAmount)
            {
                bob = bobAmount;
                decreasing = true;
            }
        }
        transform.position = v + Vector3.up * bob;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        player.health = Mathf.Clamp(player.health + healAmount, 0f, 100f);
        Destroy(gameObject);
    }
}
