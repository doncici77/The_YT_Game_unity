using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatChoice : MonoBehaviour
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
    public TextMeshProUGUI statText; // 선택지 텍스트
    public StatChoiceGroup statChoiceGroup; // 선택지 그룹

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
                statChoiceGroup.ChooseStat(gameObject); // 선택지 그룹에서 선택 처리
            }
        }
    }

    void ApplyStat(PlayerController player)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        switch (statType)
        {
            case StatType.Health:
                player.health += amount;
                player.maxHealth += amount;
                gameManager?.uiManager.UpdateHealthUI(player.health);
                break;
            case StatType.AttackSpeed:
                player.fireRate += amount;
                gameManager?.uiManager.UpdateAttackSpeedUI(player.fireRate);
                break;
            case StatType.AttackDamage:
                player.bulletDamage += amount;
                gameManager?.uiManager.UpdateAttackDamageUI(player.bulletDamage);
                break;
            case StatType.AttackRange:
                player.attackRange += amount;
                gameManager?.uiManager.UpdateAttackRangeUI(player.attackRange);
                break;
        }

        AudioManager.instance.PlaySound(AudioManager.instance.choiceSound); // 선택지 지나갈 때 사운드 재생
    }

    public void UpdateStatText()
    {
        if (statText != null)
        {
            switch (statType)
            {
                case StatType.Health:
                    statText.text = "Health + " + amount.ToString();
                    break;
                case StatType.AttackSpeed:
                    statText.text = "Speed + " + amount.ToString();
                    break;
                case StatType.AttackDamage:
                    statText.text = "Damage + " + amount.ToString();
                    break;
                case StatType.AttackRange:
                    statText.text = "Range + " + amount.ToString();
                    break;
            }
        }
    }
}
