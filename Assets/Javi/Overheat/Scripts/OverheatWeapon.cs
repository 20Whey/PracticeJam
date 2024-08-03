using System;
using System.Collections;
using UnityEngine;


public enum BulletType { Age, Deage }

public class OverheatWeapon : MonoBehaviour {

    public event Action<int> OnBulletFired;
    public event Action<BulletType> OnWeaponOverheated;

    [SerializeField] private int maxBulletsFired;
    [SerializeField] private int overheatCooldownTime = 5;

    private int currentBulletsFired;
    private BulletType bulletType;

    private GunScript gun;

    public int MaxBulletsFired => maxBulletsFired;

    private void Awake() {
        gun = GetComponent<GunScript>();
        bulletType = BulletType.Age;

        currentBulletsFired = 0;
    }

    private void OnEnable() {
        gun.OnBulletShot += BuylletFired;
    }
    private void OnDisable() {
        gun.OnBulletShot -= BuylletFired;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            bulletType = EnumExtensions.GetNextEnumValue(bulletType);
            //Note: dany, put here the sound for when you change the bullet Type
        }
    }

    private void BuylletFired() {
        switch (bulletType) {
            case BulletType.Age:
                if (currentBulletsFired <= maxBulletsFired) {
                    currentBulletsFired++;
                    OnBulletFired?.Invoke(currentBulletsFired);

                    if (currentBulletsFired == maxBulletsFired) {
                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;

            case BulletType.Deage:
                if (currentBulletsFired >= -maxBulletsFired) {
                    currentBulletsFired--;
                    OnBulletFired?.Invoke(currentBulletsFired);

                    if (currentBulletsFired == -maxBulletsFired) {
                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;
            default:
                break;
        }
    }

    IEnumerator OverheatCooldown(int seconds) {
        gun.ChangeOverheated(true);
        yield return new WaitForSeconds(seconds);

        currentBulletsFired = 0;
        OnBulletFired?.Invoke(currentBulletsFired);

        gun.ChangeOverheated(false);
    }
}
