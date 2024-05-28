using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText; // 플레이어의 체력 텍스트 UI
    public TextMeshProUGUI attackSpeedText; // 플레이어의 공격 속도 텍스트 UI
    public TextMeshProUGUI attackDamageText; // 플레이어의 공격 데미지 텍스트 UI
    public TextMeshProUGUI attackRangeText; // 플레이어의 공격 거리 텍스트 UI
    public TextMeshProUGUI scoreText; // 점수 텍스트 UI
    public GameObject gameOverPanel; // 게임 오버 패널 UI
    public TextMeshProUGUI gameOverText; // 게임 오버 텍스트 UI

    private int score = 0;
    private bool isGameOver = false; // 게임 오버 상태 체크 변수

    void Start()
    {
        UpdateHealthUI(100); // 초기 체력 설정
        UpdateAttackSpeedUI(1); // 초기 공격 속도 설정
        UpdateAttackDamageUI(10); // 초기 공격 데미지 설정
        UpdateAttackRangeUI(5); // 초기 공격 거리 설정
        UpdateScoreUI(0); // 초기 점수 설정
        gameOverPanel.SetActive(false); // 게임 오버 패널 숨기기
    }

    void Update()
    {
        // 게임 오버 상태에서 "R" 키를 누르면 게임 재시작
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void UpdateHealthUI(float health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    public void UpdateAttackSpeedUI(float attackSpeed)
    {
        attackSpeedText.text = "Speed: " + attackSpeed.ToString();
    }

    public void UpdateAttackDamageUI(float attackDamage)
    {
        attackDamageText.text = "Damage: " + attackDamage.ToString();
    }

    public void UpdateAttackRangeUI(float attackRange)
    {
        attackRangeText.text = "Range: " + attackRange.ToString();
    }

    public void UpdateScoreUI(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ShowGameOver(string message)
    {
        gameOverText.text = message;
        gameOverPanel.SetActive(true);
        isGameOver = true; // 게임 오버 상태로 설정
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 재시작 시 시간을 정상 속도로 되돌림
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬을 다시 로드
    }

    public void UpdateUI(float health, float attackSpeed, float attackDamage, float attackRange, int score)
    {
        UpdateHealthUI(health);
        UpdateAttackSpeedUI(attackSpeed);
        UpdateAttackDamageUI(attackDamage);
        UpdateAttackRangeUI(attackRange);
        UpdateScoreUI(score);
    }
}
