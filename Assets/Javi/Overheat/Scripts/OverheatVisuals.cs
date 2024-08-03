using UnityEngine;

public class OverheatVisuals : MonoBehaviour {

    [SerializeField] private OverheatWeapon overheatWeapon;
    [Space]
    [SerializeField] private float minMaxRotationAngle;

    private float rotationInterval;

    private float initialXRotation;
    private float initialYRotation;
    private float initialZRotation;

    private void Awake() {
        initialXRotation = transform.rotation.eulerAngles.x;
        initialYRotation = transform.rotation.eulerAngles.y;
        initialZRotation = transform.rotation.eulerAngles.z;

        rotationInterval = (initialZRotation + minMaxRotationAngle) / overheatWeapon.MaxBulletsFired;
    }

    private void OnEnable() {
        overheatWeapon.OnBulletFired += OverheatWeapon_OnBulletFired;
    }

    private void OnDisable() {
        overheatWeapon.OnBulletFired -= OverheatWeapon_OnBulletFired;
    }

    private void OverheatWeapon_OnBulletFired(int currentBullets) {
        float newZRotation = rotationInterval * currentBullets;

        transform.localRotation = Quaternion.Euler(initialXRotation, initialYRotation, newZRotation);
    }
}
