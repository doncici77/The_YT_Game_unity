using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f; // 최대 체력
    public float moveSpeed = 5f;
    public float slowMoveSpeed = 2f; // 전진 속도가 줄어든 상태의 속도
    public float strafeSpeed = 2f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 1f;
    public float bulletDamage = 10f;
    public float attackRange = 10f;

    private float nextFireTime = 0f;
<<<<<<< Updated upstream:The YT Game/Assets/Scrept/PlayerController.cs

    void Update()
    {
=======
    private bool isGameOver = false;
    private bool isSlowed = false; // 전진 속도가 줄어든 상태인지 여부

    void Update()
    {
        if (isGameOver)
            return;

        // 전진 속도 조절
        float currentMoveSpeed = isSlowed ? slowMoveSpeed : moveSpeed;

>>>>>>> Stashed changes:The YT Game/Assets/Scripts/PlayerController.cs
        // 자동 전진
        transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

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

            // 충돌 시 물리적 힘 제거
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
<<<<<<< Updated upstream:The YT Game/Assets/Scrept/PlayerController.cs
=======
        else if (collision.collider.CompareTag("Boss"))
        {
            Boss boss = collision.collider.GetComponent<Boss>();
            if (boss != null)
            {
                TakeDamage(boss.health); // 보스의 남은 체력만큼 데미지 입기
                boss.TakeDamage(boss.health); // 플레이어와 충돌 시 보스도 데미지 입기 (선택 사항)
            }

            // 충돌 시 물리적 힘 제거
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
>>>>>>> Stashed changes:The YT Game/Assets/Scripts/PlayerController.cs
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
            gameManager.uiManager.UpdateHealthUI(health); // 'this'를 사용하여 현재 인스턴스의 health 참조
        }
    }

    public void HealToFull()
    {
        health = maxHealth;
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

    public void SetSlowed(bool slowed)
    {
        isSlowed = slowed;
    }
}
