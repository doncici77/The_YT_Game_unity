using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public PlayerController player; // 플레이어 오브젝트를 참조합니다.
    public UIManager uiManager; // UI 매니저 오브젝트를 참조합니다.
    public GameObject statChoicePrefab; // 스탯 선택지 프리팹을 참조합니다.
    public GameObject enemyPrefab; // 적 프리팹을 참조합니다.
    public GameObject statBoxPrefab; // 스탯 박스 프리팹을 참조합니다.
    public List<GameObject> bossPrefabs; // 보스 프리팹 리스트를 참조합니다.
    public Transform leftSpawnPoint; // 왼쪽 스폰 포인트를 참조합니다.
    public Transform rightSpawnPoint; // 오른쪽 스폰 포인트를 참조합니다.
    public Transform enemySpawnPoint; // 적 스폰 포인트를 참조합니다.
    public Transform bossSpawnPoint; // 보스 스폰 포인트를 참조합니다.
    public float initialStatSpawnDistance = 20f; // 스탯 선택지 최초 생성 거리
    public float statSpawnInterval = 20f; // 스탯 선택지 생성 간격
    public float initialEnemySpawnDistance = 15f; // 적 최초 생성 거리
    public float enemySpawnInterval = 15f; // 적 생성 간격
    public float bossSpawnInterval = 100f; // 보스 생성 간격
    public float healthIncreaseValue = 5f; // 스탯 선택지에서 체력 증가량
    public float attackSpeedIncreaseValue = 0.5f; // 스탯 선택지에서 공격 속도 증가량
    public float attackDamageIncreaseValue = 10f; // 스탯 선택지에서 공격 데미지 증가량
    public float attackRangeIncreaseValue = 1f; // 스탯 선택지에서 공격 거리 증가량
    public float spawnBufferDistance = 10f; // 플레이어와 스폰 위치 사이의 거리
    public float maxSpawnDistance = 120f; // 최대 스폰 거리
    public float initialEnemyHealth = 100f; // 초기 적 체력
    public float enemyHealthIncreaseInterval = 30f; // 적 체력 증가 간격 (초)
    public float enemyHealthIncrement = 10f; // 적 체력 증가량

    private float nextStatSpawnZ;
    private float nextEnemySpawnZ;
    private float nextBossSpawnZ;
    private int bossCount = 0; // 등장한 보스 수
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
            uiManager.UpdateUI(player.health, player.fireRate, player.bulletDamage, player.attackRange, 0);
        }
        else
        {
            Debug.LogError("UIManager is not assigned in the inspector and cannot be found in the scene.");
        }

        AudioManager.instance.PlayBackgroundMusic(1); // 첫 번째 배경음 재생

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
            uiManager.UpdateAttackSpeedUI(player.fireRate);
            uiManager.UpdateAttackDamageUI(player.bulletDamage);
            uiManager.UpdateAttackRangeUI(player.attackRange);
        }

        AudioManager.instance.PlaySound(AudioManager.instance.choiceSound); // 선택지 지나갈 때 사운드 재생
    }

    public void EnemyDefeated(int points)
    {
        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(points);
        }

        // 적이 죽을 때 사운드 재생 (이미 Enemy 스크립트에서 처리)
    }

    public void BossDefeated()
    {
        bossCount++;
        switch (bossCount)
        {
            case 1:
                AudioManager.instance.PlayBackgroundMusic(2);
                break;
            case 2:
                AudioManager.instance.PlayBackgroundMusic(3);
                break;
            case 3:
                AudioManager.instance.PlayBackgroundMusic(4);
                break;
        }

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

        // 왼쪽 선택지 생성
        statChoiceGroup.statChoices[0] = CreateStatChoice(new Vector3(leftSpawnPoint.position.x, leftSpawnPoint.position.y, nextStatSpawnZ), statChoiceGroup, true);

        // 오른쪽 선택지 생성
        statChoiceGroup.statChoices[1] = CreateStatChoice(new Vector3(rightSpawnPoint.position.x, rightSpawnPoint.position.y, nextStatSpawnZ), statChoiceGroup, false);
    }

    private GameObject CreateStatChoice(Vector3 spawnPosition, StatChoiceGroup group, bool isLeft)
    {
        GameObject statChoice = Instantiate(statChoicePrefab, spawnPosition, Quaternion.identity);
        StatChoice statChoiceScript = statChoice.GetComponent<StatChoice>();
        statChoiceScript.statChoiceGroup = group;

        // 좌우 선택지의 스탯을 다르게 설정
        if (isLeft)
        {
            statChoiceScript.statType = (StatChoice.StatType)Random.Range(0, System.Enum.GetValues(typeof(StatChoice.StatType)).Length);
        }
        else
        {
            // 왼쪽 선택지와 다른 스탯을 설정
            do
            {
                statChoiceScript.statType = (StatChoice.StatType)Random.Range(0, System.Enum.GetValues(typeof(StatChoice.StatType)).Length);
            } while (statChoiceScript.statType == group.statChoices[0].GetComponent<StatChoice>().statType);
        }

        // 각 스탯 타입에 따른 증가량 설정
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
        statChoiceScript.UpdateStatText();
        return statChoice;
    }

    public void GenerateStatBox(Vector3 position)
    {
        GameObject statBox = Instantiate(statBoxPrefab, position, Quaternion.identity);
        StatBox statBoxScript = statBox.GetComponent<StatBox>();
        StatBox.StatType statType = (StatBox.StatType)Random.Range(0, System.Enum.GetValues(typeof(StatBox.StatType)).Length);
        statBoxScript.statType = statType;

        // 각 스탯 타입에 따른 증가량 설정
        switch (statType)
        {
            case StatBox.StatType.Health:
                statBoxScript.amount = healthIncreaseValue;
                break;
            case StatBox.StatType.AttackSpeed:
                statBoxScript.amount = attackSpeedIncreaseValue;
                break;
            case StatBox.StatType.AttackDamage:
                statBoxScript.amount = attackDamageIncreaseValue;
                break;
            case StatBox.StatType.AttackRange:
                statBoxScript.amount = attackRangeIncreaseValue;
                break;
        }
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
