using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int health;
    Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        health = PlayerManager.instance.player.GetComponent<Player2>().health;
        healthText = GetComponent<Text>();
        healthText.text = "Health: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealth() {
        health = PlayerManager.instance.player.GetComponent<Player2>().health;
        healthText.text = "Health: " + health.ToString();
    }
 }
