using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Player player;
    private float actualDash;
    private float delayedDash;
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
        actualDash = player.CurrentDash;
        delayedDash = Mathf.Lerp(delayedDash, actualDash, 0.1f);

        actual.fillAmount = actualDash / player.DashMax;
        delayed.fillAmount = delayedDash / player.DashMax;
    }
}
