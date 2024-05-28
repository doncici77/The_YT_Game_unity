using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage = 20f; // 총알의 데미지

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // 총알을 파괴
        }
        else
        {
            Destroy(gameObject); // 총알이 다른 오브젝트와 충돌해도 파괴
        }
    }
}