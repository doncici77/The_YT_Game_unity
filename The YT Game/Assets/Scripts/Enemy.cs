using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public int scoreValue = 10; // 적을 처치했을 때 얻는 점수
    private Transform player; // 플레이어의 위치를 저장할 변수
    public float despawnDistance = 30f; // 플레이어와의 거리

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
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EnemyDefeated(scoreValue);
        }
        Destroy(gameObject);
    }

    void UpdateHealthText()
    {
        // 건강 텍스트 업데이트
    }
}
