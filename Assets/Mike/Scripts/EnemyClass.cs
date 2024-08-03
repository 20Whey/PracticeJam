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
	public float bulletspeed;

	public EnemyAge enemyAge;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;
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

	private void Update()
	{
		if (Input.GetKeyDown("space")) { FireBullet(); }
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

	public void FireBullet()
	{
		ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation, ObjectPoolManager.PoolType.Projectiles);
	}

	public void AgeEnemy(int damage)
	{
		oldAge += damage;
		if(oldAge >= maxAge)
		{
			if (enemyAge != EnemyAge.Old)
			{
				enemyManager.SwitchObject(gameObject, "old");
			}
			else
			{
				enemyManager.SwitchObject(gameObject, "oldRD");
			}
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
				enemyManager.SwitchObject(gameObject, "young");
			}
			else
			{
				enemyManager.SwitchObject(gameObject, "youngRD");
			}
			enemyManager.enemies.Remove(gameObject);
		}
	}
}
