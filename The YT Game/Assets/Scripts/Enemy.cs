using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public int scoreValue = 10; // 적을 처치했을 때 얻는 점수
    public GameObject statBoxPrefab; // 스탯 상자 프리팹
    public float despawnDistance = 30f; // 플레이어와의 거리
    public TextMeshProUGUI healthText; // 적 체력 텍스트
    public AudioClip hitSound; // 적이 맞았을 때의 사운드
    public AudioClip deathSound; // 적이 죽었을 때의 사운드
    public float hitSoundVolume = 1.0f; // 맞았을 때 사운드 볼륨
    public float deathSoundVolume = 1.0f; // 죽었을 때 사운드 볼륨

    private Transform player; // 플레이어의 위치를 저장할 변수

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform을 가져옵니다.
        UpdateHealthText();
    }

    void Update()
    {
        // 플레이어와의 거리가 despawnDistance 이상이면 적을 제거합니다.
        if (player != null && transform.position.z < player.position.z - despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();
        
        // 적이 맞았을 때의 사운드 재생
        if (hitSound != null)
        {
            AudioHelper.PlayClipAtPoint(hitSound, transform.position, hitSoundVolume);
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        // 적이 죽었을 때의 사운드 재생
        if (deathSound != null)
        {
            AudioHelper.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EnemyDefeated(scoreValue);
            gameManager.GenerateStatBox(transform.position); // 적을 죽였을 때 스탯 박스 생성
        }

        Destroy(gameObject);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health.ToString();
        }
    }
}

