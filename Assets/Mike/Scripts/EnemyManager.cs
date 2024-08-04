using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] public List<GameObject> enemies = new List<GameObject>();
	[SerializeField] private int waveCount = 1;
	public float statMultiplier = 0.5f;
	public GameObject youngPrefab;
	public GameObject oldPrefab;
	public GameObject oldRagdollPrefab;
	public GameObject youngRagdollPrefab;
	private EnemySpawner[] enemySpawners;

	void Awake()
	{
		enemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
	}

	public void checkWaveClear()
	{ 
		if(enemies.Count == 0)
		{
			AdvanceWave(waveCount);
			waveCount += 1;
			Debug.Log(waveCount);
		}
	}

	private void AdvanceWave(int waveCount)
	{
		foreach (EnemySpawner enemySpawner in enemySpawners)
		{
			StartCoroutine(enemySpawner.SpawnEnemies());
		}
		foreach (GameObject enemy in enemies)
		{
			EnemyClass enemyStats = enemy.GetComponent<EnemyClass>();
			enemyStats.maxAge = (int)(enemyStats.maxAge * statMultiplier);
			enemyStats.enemyDamage = (int)(enemyStats.enemyDamage * statMultiplier);
			enemyStats.movementSpeed *= statMultiplier;
		}
		statMultiplier *= waveCount;
	}

	public void SwitchObject(GameObject objectToSwitch, string objectToSwitchTo)
	{
		switch (objectToSwitchTo)
		{
			case "young":
				ObjectPoolManager.SpawnObject(youngPrefab, objectToSwitch.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
				ObjectPoolManager.ReturnObjectToPool(objectToSwitch);
				break;
			case "youngRD":
				ObjectPoolManager.SpawnObject(youngRagdollPrefab, objectToSwitch.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
				ObjectPoolManager.ReturnObjectToPool(objectToSwitch);
				break;
			case "old":
				ObjectPoolManager.SpawnObject(oldPrefab, objectToSwitch.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
				ObjectPoolManager.ReturnObjectToPool(objectToSwitch);
				break;
			case "oldRD":
				ObjectPoolManager.SpawnObject(oldRagdollPrefab, objectToSwitch.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
				ObjectPoolManager.ReturnObjectToPool(objectToSwitch);
				break;
			default:
				ObjectPoolManager.ReturnObjectToPool(objectToSwitch);
				break;
		}
	}
}
