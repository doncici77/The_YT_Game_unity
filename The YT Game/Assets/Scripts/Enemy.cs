using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private Renderer renderer;
    public int scoreValue = 10; // 적을 처치했을 때 얻는 점수
    public float damage = 10f;  // 적이 플레이어에게 입히는 데미지

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        ChangeColor();
        if (health <= 0f)
        {
            Die();
        }
    }

    void ChangeColor()
    {
        float healthRatio = health / 100f;
        renderer.material.color = new Color(1f, healthRatio, healthRatio); // 체력에 비례하여 색상 변경
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
}
