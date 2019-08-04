using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Player player;
    private float actualHealth;
    private float delayedHealth;
    public Image actual;
    public Image delayed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        actualHealth = player.health;
        delayedHealth = Mathf.Lerp(delayedHealth, actualHealth, 0.1f);

        actual.fillAmount = actualHealth / 100f;
        delayed.fillAmount = delayedHealth / 100f;
    }
}
