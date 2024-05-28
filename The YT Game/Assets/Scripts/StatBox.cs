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
    public AudioClip pickupSound; // 박스를 먹었을 때의 사운드
    public float pickupSoundVolume = 1.0f; // 박스를 먹었을 때 사운드 볼륨

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

            // 박스를 먹었을 때의 사운드 재생
            if (pickupSound != null)
            {
                AudioHelper.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);
            }

            // 상자를 파괴
            Destroy(gameObject);
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
