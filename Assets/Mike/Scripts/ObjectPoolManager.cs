using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
	public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

	private GameObject _objectPoolEmptyHolder;

	private static GameObject _particleSystemsEmpty;
	private static GameObject _projectilesEmpty;
	private static GameObject _enemiesEmpty;

	public enum PoolType
	{
		ParticleSystem,
		Projectiles,
		Enemies,
		None
	}
	public static PoolType PoolingType;

	private void Awake()
	{
		SetupEmpties();
	}
	private void SetupEmpties()
	{
		_objectPoolEmptyHolder = new GameObject("Pooled Objects");

		_particleSystemsEmpty = new GameObject("Particle Effects");
		_particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

		_projectilesEmpty = new GameObject("Projectiles");
		_projectilesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

		_enemiesEmpty = new GameObject("Enemies");
		_enemiesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
	}

	public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnpos, Quaternion spawnRotation, PoolType poolType = PoolType.None)
	{
		//search through list of pools selecting pool with the lookup string matching the spawnobject
		PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

		//If the pool isn't yet created, create it
		if (pool == null)
		{
			pool = new PooledObjectInfo() { lookupString = objectToSpawn.name };
			objectPools.Add(pool);
		}

		//check if there is inactive objects in pool to use (using Linq namespace)
		GameObject spawnableObject = pool.inactiveObjects.FirstOrDefault();

		if (spawnableObject == null)
		{
			//find parent of the empty object
			GameObject parentObject = SetParentObject(poolType);

			//no in active objects in pool, create one
			spawnableObject = Instantiate(objectToSpawn, spawnpos, spawnRotation);

			if(parentObject != null)
			{
				spawnableObject.transform.SetParent(parentObject.transform);
			}
		}
		else
		{
			//found active object in pool, reactivate it
			spawnableObject.transform.position = spawnpos;
			spawnableObject.transform.rotation = spawnRotation;
			pool.inactiveObjects.Remove(spawnableObject);
			spawnableObject.SetActive(true);

			if(poolType == PoolType.Enemies)
			{
				objectToSpawn.GetComponent<OldEnemy>().oldAge = 0;
			}
		}

		return spawnableObject;
	}

	public static void ReturnObjectToPool(GameObject obj)
	{
		string objectName = obj.name.Substring(0, obj.name.Length - 7); //remove last 7 characters to delete (clone) from the name to pass

		PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectName);

		if (pool == null)
		{
			Debug.LogWarning("Trying to return object which has no pool: " + obj.name);
		}
		else
		{
			obj.SetActive(false);
			pool.inactiveObjects.Add(obj);
		}
	}

	private static GameObject SetParentObject(PoolType poolType)
	{
		switch (poolType)
		{
			case PoolType.ParticleSystem:
				return _particleSystemsEmpty;

			case PoolType.Projectiles:
				return _projectilesEmpty;

			case PoolType.Enemies:
				return _enemiesEmpty;

			case PoolType.None:
				return null;

			default:
				return null;
		}
	}
}

public class PooledObjectInfo
{
	public string lookupString;
	public List<GameObject> inactiveObjects = new List<GameObject>();
}