using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
	//Enemy Stats
	public int maxAge;
	private int currentAge;
	public int oldAge;
	public int youngAge;
	public int enemyDamage;
	public float movementSpeed;
	public float accuracy;

	public EnemyAge enemyAge;
	public GameObject youngPrefab;
	public GameObject oldPrefab;
	private EnemyManager enemyManager;

	public enum EnemyAge
	{ 
		Young,
		Old,
		Prime,
	}

	void Awake()
	{
		enemyManager = FindObjectOfType<EnemyManager>();
		enemyManager.enemies.Add(gameObject);
	}

	void OnEnable()
	{
		oldAge = 0;
		youngAge = 0;
	}
	public void TakeDamage(int damage, string damageType)
	{
		switch (damageType)
		{
			case "age":
				AgeEnemy(damage);
				break;

			case "deAge":
				DeAgeEnemy(damage);
				break;
		}
		enemyManager.checkWaveClear();
	}

	public void AgeEnemy(int damage)
	{
		oldAge += damage;
		if(oldAge >= maxAge)
		{
			if(enemyAge != EnemyAge.Old)
			{
				ObjectPoolManager.SpawnObject(oldPrefab, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
			}
			ObjectPoolManager.ReturnObjectToPool(gameObject);
			enemyManager.enemies.Remove(gameObject);
		}
	}

	public void DeAgeEnemy(int damage)
	{
		youngAge += damage;
		if (youngAge >= maxAge)
		{
			if (enemyAge != EnemyAge.Young)
			{
				ObjectPoolManager.SpawnObject(youngPrefab, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
			}
			ObjectPoolManager.ReturnObjectToPool(gameObject);
			enemyManager.enemies.Remove(gameObject);
		}
	}
}
