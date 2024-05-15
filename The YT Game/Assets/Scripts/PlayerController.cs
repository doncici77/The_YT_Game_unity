using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 5f;
    public float strafeSpeed = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 1f;
    public float bulletDamage = 10f;
    public float attackRange = 50f; // 총알 최대 거리

    private float nextFireTime = 0f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver)
            return;

        // 자동 전진
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // 좌우 이동
        float strafe = Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;
        transform.Translate(strafe, 0, 0);

        // 공격
        if (Time.time > nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
            bulletScript.maxDistance = attackRange; // 총알 최대 거리 설정
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.health); // 적의 남은 체력만큼 데미지 입기
                enemy.Die(); // 적 제거
            }
        }
        else if (collision.collider.CompareTag("Boss"))
        {
            Boss boss = collision.collider.GetComponent<Boss>();
            if (boss != null)
            {
                TakeDamage(boss.health); // 보스의 남은 체력만큼 데미지 입기
                boss.TakeDamage(boss.health); // 플레이어와 충돌 시 보스도 데미지 입기 (선택 사항)
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

        // UI 업데이트
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.uiManager.UpdateHealthUI(health);
        }
    }

    void Die()
    {
        // 플레이어가 죽었을 때의 처리
        Debug.Log("Player Died");
        isGameOver = true;
        // 게임 오버 UI 표시
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.uiManager.ShowGameOver();
        }
        // 게임 일시정지
        Time.timeScale = 0f;
    }
}
