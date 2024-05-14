using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChoice : MonoBehaviour
{
    public enum StatType { Health, AttackSpeed, AttackDamage, AttackRange }
    public StatType statType;
    public float amount = 1f;
    private bool isActive = true;
    public TMPro.TextMeshProUGUI statText;  // 텍스트 컴포넌트 참조

    void Start()
    {
        // 선택지 텍스트 설정
        if (statText != null)
        {
            switch (statType)
            {
                case StatType.Health:
                    statText.text = "HP +" + amount.ToString();
                    break;
                case StatType.AttackSpeed:
                    statText.text = "ATK SPEED +" + amount.ToString();
                    break;
                case StatType.AttackDamage:
                    statText.text = "DAMEGE +" + amount.ToString();
                    break;
                case StatType.AttackRange:
                    statText.text = "RANGE +" + amount.ToString();
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ApplyStat(statType, amount);
                isActive = false;
                gameObject.SetActive(false); // 스텟 선택지를 비활성화
            }
        }
    }
}