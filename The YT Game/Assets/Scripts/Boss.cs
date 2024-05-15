using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    public float health = 500f;
    public int scoreValue = 100; // 보스를 처치했을 때 얻는 점수
    public TextMeshProUGUI healthText; // 보스 체력 텍스트
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치
    public float fireRate = 1f; // 총알 발사 간격
    public float bulletRange = 50f; // 총알 최대 거리
    private float nextFireTime = 0f;

    void Start()
    {
        UpdateHealthText();
    }

    void Update()
    {
        // 플레이어에게 총알 발사
        if (Time.time > nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();
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
            gameManager.BossDefeated(); // 보스 처치 알림
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

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = 20f; // 보스 총알 데미지 설정
            bulletScript.maxDistance = bulletRange; // 총알 최대 거리 설정
            bulletScript.SetDirection(Vector3.back); // 총알 이동 방향을 -z 방향으로 설정
        }
    }
}
