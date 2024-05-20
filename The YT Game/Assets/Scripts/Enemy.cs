using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public int scoreValue = 10; // 적을 처치했을 때 얻는 점수
    public GameObject statBoxPrefab; // 스탯 상자 프리팹
    public float despawnDistance = 30f; // 플레이어와의 거리
    public TextMeshProUGUI healthText; // 적 체력 텍스트

    private Transform player; // 플레이어의 위치를 저장할 변수

    private Transform player; // 플레이어의 위치를 저장할 변수

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform을 가져옵니다.
        UpdateHealthText();
    }

    void Update()
    {
        // 플레이어와의 거리가 despawnDistance 이상이면 적을 제거합니다.
        if (player != null && transform.position.z < player.position.z - despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EnemyDefeated(scoreValue);
        }

        // 스탯 상자 생성
        GenerateStatBox();

        Destroy(gameObject);
    }

    void GenerateStatBox()
    {
        if (statBoxPrefab != null)
        {
            GameObject statBox = Instantiate(statBoxPrefab, transform.position, Quaternion.identity);
            StatBox statBoxScript = statBox.GetComponent<StatBox>();
<<<<<<< Updated upstream
            TextMeshProUGUI statText = statBox.GetComponentInChildren<TextMeshProUGUI>();
=======
>>>>>>> Stashed changes

            // 스탯 타입과 증가량을 랜덤으로 설정
            statBoxScript.statType = (StatBox.StatType)Random.Range(0, System.Enum.GetValues(typeof(StatBox.StatType)).Length);
            statBoxScript.amount = Random.Range(1, 5); // 예시: 1에서 5 사이의 랜덤 값
<<<<<<< Updated upstream
            statBoxScript.statText = statText; // 텍스트 컴포넌트를 할당
            statBoxScript.UpdateStatText(); // 스탯 정보를 업데이트
=======
>>>>>>> Stashed changes
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health.ToString();
        }
    }
}
