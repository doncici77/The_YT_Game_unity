using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float maxDistance = 100f; // 최대 거리
    private Vector3 startPosition;
    private Vector3 direction = Vector3.forward; // 기본 방향을 z 방향으로 설정

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // 최대 거리를 초과하면 총알 파괴
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // 충돌 시 총알 파괴
        }
        else if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // 충돌 시 총알 파괴
        }
        else if (collision.collider.CompareTag("Boss")) // 보스와 충돌 처리
        {
            Boss boss = collision.collider.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject); // 충돌 시 총알 파괴
        }
        else if (collision.collider.CompareTag("Obstacle")) // 장애물에 부딪혀도 사라짐
        {
            Destroy(gameObject); // 충돌 시 총알 파괴
        }
    }
}
