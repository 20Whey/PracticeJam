using System.Collections;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    public Rigidbody bulletRB;
    public float bulletSpeed;
    public int bulletDamage;
    public DamageType damageType;

    [SerializeField] private float destroyTime = 1f;
    private Coroutine _returnToPoolTimer;

    public enum DamageType
    {
        Young,
        Old,
        Enemy,
    }

    private void OnEnable()
    {
        bulletRB = gameObject.GetComponent<Rigidbody>();
        _returnToPoolTimer = StartCoroutine(ReturnToPoolDelayed());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyShotsSound, transform.position);
    }

    private void Update()
    {
        bulletRB.velocity = transform.forward * bulletSpeed;

    }

    private IEnumerator ReturnToPoolDelayed()
    {
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && damageType == DamageType.Enemy) {
            collision.gameObject.GetComponent<PlayerMovementScript>().playerTakeDamage(bulletDamage);
        }
        if (collision.gameObject.tag == "Enemy" && damageType == DamageType.Old) {
            collision.gameObject.transform.GetChild(0).GetComponent<EnemyClass>().TakeDamage(bulletDamage, "age");
        }
        if (collision.gameObject.tag == "Enemy" && damageType == DamageType.Young) {
            collision.gameObject.transform.GetChild(0).GetComponent<EnemyClass>().TakeDamage(bulletDamage, "deAge");
        }
    }
}
