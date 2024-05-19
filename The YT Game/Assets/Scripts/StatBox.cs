using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TextMeshProUGUI statText; // 스탯 정보 텍스트

    private void Start()
    {
        UpdateStatText();
    }

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
                player.maxHealth += amount;
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

    public void UpdateStatText() // 메서드를 public으로 변경
    {
        if (statText != null)
        {
            switch (statType)
            {
                case StatType.Health:
                    statText.text = "Health + " + amount.ToString();
                    break;
                case StatType.AttackSpeed:
                    statText.text = "Attack Speed + " + amount.ToString();
                    break;
                case StatType.AttackDamage:
                    statText.text = "Attack Damage + " + amount.ToString();
                    break;
                case StatType.AttackRange:
                    statText.text = "Attack Range + " + amount.ToString();
                    break;
            }
        }
    }
}
