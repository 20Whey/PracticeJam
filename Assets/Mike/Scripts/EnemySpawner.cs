using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;
    public Transform spawnArea;
    public int maxEnemyCount;
    [SerializeField] private int oldEnemyCount = 0;
    [SerializeField] private int youngEnemyCount = 0;
    System.Random random = new System.Random();

    public SpawnManagerScriptableObj spawnTemplate;
    private EnemyManager enemyManager;

    void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.localScale.x, spawnArea.localScale.y, spawnArea.localScale.z));
    }

    void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    void Start()
    {
        maxEnemyCount = spawnTemplate.oldEnemiesToSpawn + spawnTemplate.youngEnemiesToSpawn;
        spawnArea = this.transform;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (youngEnemyCount < maxEnemyCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            ObjectPoolManager.SpawnObject(enemyManager.youngPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
            yield return new WaitForSeconds(spawnTemplate.enemySpawnDelay);
            youngEnemyCount += 1;
        }
        while (oldEnemyCount < maxEnemyCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            ObjectPoolManager.SpawnObject(enemyManager.oldPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
            yield return new WaitForSeconds(spawnTemplate.enemySpawnDelay);
            oldEnemyCount += 1;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = (random.Next(0, (int)spawnArea.localScale.x) - spawnArea.localScale.x / 2);
        float z = (random.Next(0, (int)spawnArea.localScale.z) - spawnArea.localScale.z / 2);
        Debug.Log(x + " and " + z);
        return new Vector3(spawnArea.position.x + x, spawnArea.position.y, spawnArea.position.z + z);
    }

    //toughness ToDo
}
