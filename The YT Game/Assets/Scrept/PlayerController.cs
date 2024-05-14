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
    public float attackRange = 10f;

    private float nextFireTime = 0f;

    void Update()
    {
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
        bulletScript.damage = bulletDamage; // 총알 데미지 설정
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.health); // 적의 체력만큼 데미지
                enemy.Die(); // 적 제거
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
        // 예: 게임 오버 화면 표시, 게임 재시작 등
    }
}
