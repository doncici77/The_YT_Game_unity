using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text playerHealthText;
    public Text scoreText;
    public Text gameOverText;
    public PlayerController player;

    private int score = 0;

    void Start()
    {
        UpdateUI();
        gameOverText.gameObject.SetActive(false); // 게임 오버 문구 비활성화
    }

    void Update()
    {
        // "R" 키를 누르면 씬을 다시 로드
        if (gameOverText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; // 게임 재개
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

    public void ShowGameOver()
    {
        gameOverText.text = "Game Over! Press 'R' to Restart";
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
    }
}
