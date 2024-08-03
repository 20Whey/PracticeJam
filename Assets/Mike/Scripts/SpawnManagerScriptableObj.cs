using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn List", menuName = "Spawn Template")]
public class SpawnManagerScriptableObj : ScriptableObject
{
	public int youngEnemiesToSpawn;
	public int oldEnemiesToSpawn;
	public int enemySpawnDelay;
}
