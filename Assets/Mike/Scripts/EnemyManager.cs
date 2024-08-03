using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] public List<GameObject> enemies = new List<GameObject>();
	[SerializeField] private int waveCount = 1;
	public float statMultiplier = 0.5f;

	public void checkWaveClear()
	{ 
		if(enemies.Count == 0)
		{
			AdvanceWave(waveCount);
			statMultiplier *= waveCount;
		}
	}

	private void AdvanceWave(int waveCount)
	{
		foreach (GameObject enemy in enemies)
		{
			EnemyClass enemyStats = enemy.GetComponent<EnemyClass>();
			enemyStats.maxAge = (int)(enemyStats.maxAge * statMultiplier);
			enemyStats.enemyDamage = (int)(enemyStats.enemyDamage * statMultiplier);
			enemyStats.movementSpeed *= statMultiplier;
		}
	}
}
