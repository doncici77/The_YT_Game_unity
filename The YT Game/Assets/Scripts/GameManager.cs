using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public UIManager uiManager;
    public GameObject statChoicePrefab;
    public GameObject enemyPrefab;
    public List<GameObject> bossPrefabs;  // 보스 프리팹 리스트
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform bossSpawnPoint;  // 보스 스폰 포인트
    public float initialStatSpawnDistance = 20f;  // 스텟 선택지 최초 생성 거리
    public float statSpawnInterval = 20f;  // 스텟 선택지 생성 간격
    public float initialEnemySpawnDistance = 15f;  // 적 최초 생성 거리
    public float enemySpawnInterval = 15f;  // 적 생성 간격
    public float bossSpawnInterval = 100f;  // 보스 생성 간격
    public float healthIncreaseValue = 5f;
    public float attackSpeedIncreaseValue = 0.5f;
    public float attackDamageIncreaseValue = 10f;
    public float attackRangeIncreaseValue = 1f;
    public float spawnBufferDistance = 10f; // 플레이어와 스폰 위치 사이의 거리
    public float maxSpawnDistance = 120f; // 최대 스폰 거리
    public float initialEnemyHealth = 100f; // 초기 적 체력
    public float enemyHealthIncreaseInterval = 30f; // 적 체력 증가 간격 (초)
    public float enemyHealthIncrement = 10f; // 적 체력 증가량

    private float nextStatSpawnZ;
    private float nextEnemySpawnZ;
    private float nextBossSpawnZ;
    private int bossCount = 0;  // 등장한 보스 수
    private float currentEnemyHealth;
    private float timeSinceLastIncrease;

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

        // 초기화
        nextStatSpawnZ = player.transform.position.z + initialStatSpawnDistance;
        nextEnemySpawnZ = player.transform.position.z + initialEnemySpawnDistance;
        nextBossSpawnZ = player.transform.position.z + bossSpawnInterval;
        currentEnemyHealth = initialEnemyHealth;
        timeSinceLastIncrease = 0f;

        // 초기 스폰 위치부터 최대 스폰 거리까지 스텟 선택지와 적 생성
        while (nextStatSpawnZ <= maxSpawnDistance || nextEnemySpawnZ <= maxSpawnDistance)
        {
            if (nextStatSpawnZ <= maxSpawnDistance)
            {
                GenerateStatChoices();
                nextStatSpawnZ += statSpawnInterval;
            }

            if (nextEnemySpawnZ <= maxSpawnDistance)
            {
                GenerateEnemy();
                nextEnemySpawnZ += enemySpawnInterval;
            }
        }
    }

    void Update()
    {
        // 적 체력 증가 로직
        timeSinceLastIncrease += Time.deltaTime;
        if (timeSinceLastIncrease >= enemyHealthIncreaseInterval)
        {
            currentEnemyHealth += enemyHealthIncrement;
            timeSinceLastIncrease = 0f;
        }

        // 플레이어가 스폰 버퍼 거리까지 도달했을 때 추가 스폰
        if (player.transform.position.z + spawnBufferDistance >= nextStatSpawnZ)
        {
            GenerateStatChoices();
            nextStatSpawnZ += statSpawnInterval;
        }

        if (player.transform.position.z + spawnBufferDistance >= nextEnemySpawnZ)
        {
            GenerateEnemy();
            nextEnemySpawnZ += enemySpawnInterval;
        }

        // 보스 스폰
        if (bossCount < bossPrefabs.Count && player.transform.position.z + spawnBufferDistance >= nextBossSpawnZ)
        {
            GenerateBoss();
            nextBossSpawnZ += bossSpawnInterval;
        }
    }

    public void ApplyStat(StatChoice.StatType statType, float amount)
    {
        switch (statType)
        {
            case StatChoice.StatType.Health:
                player.health += amount;
                player.maxHealth += amount; // 최대 체력도 증가
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

    public void BossDefeated()
    {
        bossCount++;
        if (bossCount >= bossPrefabs.Count)
        {
            EndGame();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! The boss's health was greater than yours.");
        if (uiManager != null)
        {
            uiManager.ShowGameOver("Game Over! The boss's health was greater than yours.");
        }
        Time.timeScale = 0f; // 게임 일시정지
    }

    private void GenerateStatChoices()
    {
        GameObject statChoiceGroupObj = new GameObject("StatChoiceGroup");
        StatChoiceGroup statChoiceGroup = statChoiceGroupObj.AddComponent<StatChoiceGroup>();
        statChoiceGroup.statChoices = new GameObject[2];

        statChoiceGroup.statChoices[0] = CreateStatChoice(new Vector3(leftSpawnPoint.position.x, leftSpawnPoint.position.y, nextStatSpawnZ), statChoiceGroup);
        statChoiceGroup.statChoices[1] = CreateStatChoice(new Vector3(rightSpawnPoint.position.x, rightSpawnPoint.position.y, nextStatSpawnZ), statChoiceGroup);
    }

    private GameObject CreateStatChoice(Vector3 spawnPosition, StatChoiceGroup group)
    {
        GameObject statChoice = Instantiate(statChoicePrefab, spawnPosition, Quaternion.identity);
        StatChoice statChoiceScript = statChoice.GetComponent<StatChoice>();
        statChoiceScript.statType = (StatChoice.StatType)Random.Range(0, System.Enum.GetValues(typeof(StatChoice.StatType)).Length);
        statChoiceScript.statChoiceGroup = group;

        // 각 스텟 타입에 따른 증가량 설정
        switch (statChoiceScript.statType)
        {
            case StatChoice.StatType.Health:
                statChoiceScript.amount = healthIncreaseValue;
                break;
            case StatChoice.StatType.AttackSpeed:
                statChoiceScript.amount = attackSpeedIncreaseValue;
                break;
            case StatChoice.StatType.AttackDamage:
                statChoiceScript.amount = attackDamageIncreaseValue;
                break;
            case StatChoice.StatType.AttackRange:
                statChoiceScript.amount = attackRangeIncreaseValue;
                break;
        }
        return statChoice;
    }

    private void GenerateEnemy()
    {
        // 랜덤하게 왼쪽이나 오른쪽 선택
        Transform spawnPoint = Random.value > 0.5f ? leftSpawnPoint : rightSpawnPoint;
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, nextEnemySpawnZ);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.health = currentEnemyHealth;
        }
    }

    private void GenerateBoss()
    {
        if (bossCount < bossPrefabs.Count)
        {
            Vector3 spawnPosition = new Vector3(bossSpawnPoint.position.x, bossSpawnPoint.position.y, nextBossSpawnZ);
            Instantiate(bossPrefabs[bossCount], spawnPosition, Quaternion.identity);
        }
    }

    private void EndGame()
    {
        // 게임 엔딩 처리
        Debug.Log("Game Ended! You defeated all bosses!");
        // 게임 오버 UI 표시 등 추가적인 엔딩 처리를 여기에 작성
        // 예: 게임 오버 화면 표시, 게임 재시작 버튼 등
        if (uiManager != null)
        {
            uiManager.ShowGameOver("Game Ended! You defeated all bosses!");
        }
    }
}
