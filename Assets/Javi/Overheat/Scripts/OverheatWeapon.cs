using System;
using System.Collections;
using UnityEngine;


public enum BulletType { Age, Deage }

public class OverheatWeapon : MonoBehaviour {

    public GameObject? bull;
    public event Action<int, float> OnBulletFired;
    public event Action<BulletType> OnWeaponOverheated;

    [SerializeField] private int maxBulletsFired;
    [SerializeField] private int overheatCooldownTime = 5;

    private int currentBulletsFired;
    private BulletType bulletType;

    private GunScript gun;

    public int MaxBulletsFired => maxBulletsFired;

    private void Awake() {
        gun = gameObject.GetComponent<GunScript>();
        bulletType = BulletType.Age;
        currentBulletsFired = 0;
        bull = null;
    }

    private void OnEnable() {
        gun.OnBulletShot += BuylletFired;
    }
    private void OnDisable() {
        gun.OnBulletShot -= BuylletFired;
    }

    private void Update() {
        bull = gameObject.GetComponent<GunScript>().bullet;

       /* if (Input.GetKeyDown(KeyCode.L)) {
            bulletType = EnumExtensions.GetNextEnumValue(bulletType);
            //Note: dany, put here the sound for when you change the bullet Type
        }*/
    }

    private void BuylletFired() {
        switch (bull.name) {
            case "AgeBullet":
                        Debug.Log("A");

                if (currentBulletsFired <= maxBulletsFired) {
                    currentBulletsFired++;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    if (currentBulletsFired == 5) {
                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;

            default:
            Debug.Log("B");
                if (currentBulletsFired >= -maxBulletsFired) {
                    currentBulletsFired--;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    if (currentBulletsFired < -5) {
                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;
         
        }
    }

    IEnumerator OverheatCooldown(int seconds) {
        gun.ChangeOverheated(true);

        currentBulletsFired = 0;
        OnBulletFired?.Invoke(currentBulletsFired, seconds);

        yield return new WaitForSeconds(seconds);
        gun.ChangeOverheated(false);
    }
}
