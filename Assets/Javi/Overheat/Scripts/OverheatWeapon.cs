using System;
using UnityEngine;


public enum BulletType { age, deage }

public class OverheatWeapon : MonoBehaviour {

    public event Action<BulletType> OnWeaponOverheated;

    [SerializeField] private int maxBulletsFired;

    private int currentBulletsFired;
    private int thresholdBeforeOverheat = 5;

    private BulletType bulletType;

    private GunScript gun;

    private void Awake() {
        gun = GetComponent<GunScript>();
        bulletType = BulletType.age;

        currentBulletsFired = 0;
    }

    private void OnEnable() {
        gun.OnBulletShot += BuylletFired;
        OnWeaponOverheated += OverheatWeapon_OnWeaponOverheated;
    }

    private void OverheatWeapon_OnWeaponOverheated(BulletType obj) {
        Debug.Log($"Weapon Overheated by {obj}");
    }

    private void OnDisable() {
        gun.OnBulletShot -= BuylletFired;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            bulletType = EnumExtensions.GetNextEnumValue(bulletType);
            Debug.Log($"Bullet changed to {bulletType}");
        }
    }

    private void BuylletFired() {
        switch (bulletType) {
            case BulletType.age:
                if (currentBulletsFired <= maxBulletsFired) {
                    currentBulletsFired++;

                    if (currentBulletsFired == maxBulletsFired) {
                        OnWeaponOverheated?.Invoke(BulletType.age);
                    }
                }
                break;
            case BulletType.deage:
                if (currentBulletsFired >= -maxBulletsFired) {
                    currentBulletsFired--;

                    if (currentBulletsFired == -maxBulletsFired) {
                        OnWeaponOverheated?.Invoke(BulletType.deage);
                    }
                }
                break;
            default:
                break;
        }
    }
}
