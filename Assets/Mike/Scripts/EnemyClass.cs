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
	[SerializeField] private float shootingCooldown = 5;
	private float cooldownTimer;

	public EnemyAge enemyAge;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;
	public Transform playerPosition;
	private GameObject enemyParent;
	private EnemyManager enemyManager;

	public enum EnemyAge
	{ 
		Young,
		Old,
		Prime,
	}

	void Awake()
	{
		enemyParent = gameObject.transform.parent.gameObject;
		enemyManager = FindObjectOfType<EnemyManager>();
		playerPosition = FindObjectOfType<PlayerMovementScript>().gameObject.transform;

		enemyManager.enemies.Add(enemyParent);
	}

	void OnEnable()
	{
		oldAge = 0;
		youngAge = 0;
	}

	private void Update()
	{
		Vector3 playerPos = new Vector3(playerPosition.position.x, gameObject.transform.position.y, playerPosition.position.z);
		transform.LookAt(playerPos);
		DealDamage();
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

	public void DealDamage()
	{
		if (enemyAge == EnemyAge.Old)
		{
			cooldownTimer -= Time.deltaTime;
			if (cooldownTimer > 0) return;
			cooldownTimer = shootingCooldown;

			ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation, ObjectPoolManager.PoolType.Projectiles);
		}
		if (enemyAge == EnemyAge.Young)
		{
			cooldownTimer -= Time.deltaTime;
			if (cooldownTimer > 0) return;
			cooldownTimer = shootingCooldown;

			//melee attack
		}
	}

	public void AgeEnemy(int damage)
	{
		oldAge += damage;
		if(oldAge >= maxAge)
		{
			if (enemyAge != EnemyAge.Old)
			{
				enemyManager.SwitchObject(enemyParent, "old");
			}
			else
			{
				enemyManager.SwitchObject(enemyParent, "oldRD");
			}
			enemyManager.enemies.Remove(enemyParent);
		}
	}

	public void DeAgeEnemy(int damage)
	{
		youngAge += damage;
		if (youngAge >= maxAge)
		{
			if (enemyAge != EnemyAge.Young)
			{
				enemyManager.SwitchObject(enemyParent, "young");
			}
			else
			{
				enemyManager.SwitchObject(enemyParent, "youngRD");
			}
			enemyManager.enemies.Remove(enemyParent);
		}
	}
}
