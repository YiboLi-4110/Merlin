using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    public string prefabPath = "Enemies/BaseEnemy"; // Resources文件夹下的路径
    public Vector2 spawnAreaCenter;
    public Vector2 spawnAreaSize = new Vector2(20, 20);
    public int totalEnemies = 10;
    public float spawnInterval = 3f;
    public float minDistanceFromPlayer = 5f;

    [Header("调试视图")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.red;

    private Transform playerTransform;
   

    
    private GameObject enemyPrefab;
    private int spawnedCount;


    void Start()
    {
        LoadEnemyPrefab();
        StartCoroutine(SpawnRoutine());
    }

    void LoadEnemyPrefab()
    {
        // 从Resources文件夹加载预制体
        enemyPrefab = Resources.Load<GameObject>(prefabPath);
        
        if(enemyPrefab == null)
        {
            Debug.LogError($"无法在路径 Resources/{prefabPath} 找到预制体");
            enabled = false;
            return;
        }

        // 验证预制体组件
        if(!enemyPrefab.GetComponent<EnemyAI>()) // 示例验证敌人AI组件
        {
            Debug.LogWarning("预制体缺少必要组件：EnemyAI");
        }
    }

    IEnumerator SpawnRoutine()
    {
        while(spawnedCount < totalEnemies)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
            spawnedCount++;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = GetValidSpawnPosition();
        if(spawnPos == Vector2.negativeInfinity) return;

        // 实例化预制体并保持原始设置
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
        ApplyOriginalPrefabSettings(enemy);
    }

    void ApplyOriginalPrefabSettings(GameObject instance)
    {
        // 保持原始激活状态
        instance.SetActive(enemyPrefab.activeSelf);
        
        // 保持所有组件状态
        MonoBehaviour[] prefabComponents = enemyPrefab.GetComponents<MonoBehaviour>();
        MonoBehaviour[] instanceComponents = instance.GetComponents<MonoBehaviour>();
        
        for(int i=0; i<prefabComponents.Length; i++)
        {
            if(i < instanceComponents.Length)
            {
                instanceComponents[i].enabled = prefabComponents[i].enabled;
            }
        }
    }

    Vector2 GetValidSpawnPosition()
    {
        int attempts = 0;
        const int maxAttempts = 30;

        while(attempts < maxAttempts)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2),
                Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2)
            ) + spawnAreaCenter;

            if(playerTransform != null && 
               Vector2.Distance(randomPoint, playerTransform.position) < minDistanceFromPlayer)
            {
                attempts++;
                continue;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPoint, 1f);
            bool isValid = true;
            foreach(Collider2D col in colliders)
            {
                if(col.CompareTag("Enemy"))
                {
                    isValid = false;
                    break;
                }
            }

            if(isValid) return randomPoint;
            attempts++;
        }

        Debug.LogWarning("未找到有效生成位置");
        return Vector2.negativeInfinity;
    }

    void OnDrawGizmos()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;
        Vector3 center = new Vector3(spawnAreaCenter.x, spawnAreaCenter.y, 0);
        Vector3 size = new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
        
    }
