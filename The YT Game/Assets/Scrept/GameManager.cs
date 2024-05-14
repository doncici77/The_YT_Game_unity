using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public UIManager uiManager;

    void Start()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }
        else
        {
            Debug.LogError("UIManager is not assigned in the inspector and cannot be found in the scene.");
        }
    }

    public void ApplyStat(StatChoice.StatType statType, float amount)
    {
        switch (statType)
        {
            case StatChoice.StatType.Health:
                player.health += amount;
                break;
            case StatChoice.StatType.AttackSpeed:
                player.fireRate += amount;
                break;
            case StatChoice.StatType.AttackDamage:
                player.bulletDamage += amount;
                break;
            case StatChoice.StatType.AttackRange:
                player.attackRange += amount;
                break;
        }
        if (uiManager != null)
        {
            uiManager.UpdateHealthUI(player.health);
        }
    }

    public void EnemyDefeated(int points)
    {
        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(points);
        }
    }
}
