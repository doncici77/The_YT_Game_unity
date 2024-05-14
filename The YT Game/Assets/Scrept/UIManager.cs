using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerHealthText;
    public Text scoreText;
    public PlayerController player;

    private int score = 0;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateHealthUI(float health)
    {
        playerHealthText.text = "Health: " + health.ToString();
    }

    public void UpdateScoreUI(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateUI()
    {
        playerHealthText.text = "Health: " + player.health.ToString();
        scoreText.text = "Score: " + score.ToString();
    }
}
