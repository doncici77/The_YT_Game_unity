using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBox : MonoBehaviour
{
    public enum StatType
    {
        Health,
        AttackSpeed,
        AttackDamage,
        AttackRange
    }

    public StatType statType;
    public float amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyStat(player);
            }

            // 상자를 파괴
            Destroy(gameObject);
        }
    }

    void ApplyStat(PlayerController player)
    {
        switch (statType)
        {
            case StatType.Health:
                player.health += amount;
                break;
            case StatType.AttackSpeed:
                player.fireRate += amount;
                break;
            case StatType.AttackDamage:
                player.bulletDamage += amount;
                break;
            case StatType.AttackRange:
                player.attackRange += amount;
                break;
        }

        // UI 업데이트
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && gameManager.uiManager != null)
        {
            gameManager.uiManager.UpdateHealthUI(player.health);
        }
    }
}
