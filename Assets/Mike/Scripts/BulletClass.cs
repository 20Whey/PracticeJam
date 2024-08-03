using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
	public Rigidbody bulletRB;
	public float bulletSpeed;

	private void Update()
	{
		bulletRB.velocity = transform.forward * bulletSpeed;
	}
}
