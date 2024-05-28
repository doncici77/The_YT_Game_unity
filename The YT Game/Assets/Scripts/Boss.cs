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
    public float moveSpeed = 2f; // 보스의 좌우 이동 속도
    public float moveRange = 5f; // 좌우 이동 범위
    public float slowDistance = 10f; // 플레이어와의 거리
    public float despawnDistance = 10f; // 플레이어를 지나쳤을 때의 거리
    public AudioClip hitSound; // 보스가 맞았을 때의 사운드
    public AudioClip fireSound; // 총알 발사 사운드 클립

    private float nextFireTime = 0f;
    private Vector3 initialPosition;
    private bool movingRight = true;
    private Transform player;
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform을 가져옵니다.
        audioSource = GetComponent<AudioSource>();
        UpdateHealthText();
    }

    void Update()
    {
        // 좌우로 움직이기
        Move();

        // 플레이어에게 총알 발사
        if (Time.time > nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // 플레이어와의 거리 확인
        if (Vector3.Distance(transform.position, player.position) < slowDistance)
        {
            player.GetComponent<PlayerController>().SetSlowed(true); // 플레이어 전진 속도 줄이기
        }

        // 플레이어를 지나쳤는지 확인
        if (player != null && transform.position.z < player.position.z - despawnDistance)
        {
            GameOver();
        }
    }

    void Move()
    {
        float moveDirection = movingRight ? 1 : -1;
        transform.Translate(Vector3.right * moveSpeed * moveDirection * Time.deltaTime);

        if (movingRight && transform.position.x > initialPosition.x + moveRange)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x < initialPosition.x - moveRange)
        {
            movingRight = true;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();
        
        // 보스가 맞았을 때의 사운드 재생
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (gameManager != null && playerController != null)
        {
            if (health > playerController.health)
            {
                gameManager.GameOver();
            }
            else
            {
                playerController.HealToFull(); // 플레이어 체력 전부 채우기
                gameManager.EnemyDefeated(scoreValue);
                gameManager.BossDefeated(); // 보스 처치 알림
            }
        }

        playerController.SetSlowed(false); // 플레이어 전진 속도 원래대로

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
        FireBullet(Vector3.back); // 가운데
    }

    void FireBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
        BossBullet bulletScript = bullet.GetComponent<BossBullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = 20f; // 보스 총알 데미지 설정
        }

        // 총알 발사 사운드 재생
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    void GameOver()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}
