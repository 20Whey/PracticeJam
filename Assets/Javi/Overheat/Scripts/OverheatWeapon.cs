using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;


public enum BulletType { Age, Deage }

public class OverheatWeapon : MonoBehaviour
{

    public event Action<int, float> OnBulletFired;
    public event Action<BulletType> OnWeaponOverheated;

    [SerializeField] private int warningBulletsFired;
    [SerializeField] private int maxBulletsFired;
    [SerializeField] private int overheatCooldownTime = 5;

    private int currentBulletsFired;
    private BulletType bulletType;

    private GunScript gun;

    private bool isGunOverheated = false;

    float gaugeChange = 0;

    float overheatingVol = 1.0f;

    Vector3 offset = new Vector3(0, 0, 1f);

    public int MaxBulletsFired => maxBulletsFired;

    private void Awake()
    {
        gun = GetComponent<GunScript>();
        bulletType = BulletType.Age;

        currentBulletsFired = 0;

        gaugeSound = AudioManager.instance.CreateInstance(FMODEvents.instance.gaugeSound);
        preOverheatSound = AudioManager.instance.CreateInstance(FMODEvents.instance.overheatingSound);
        overheatedSound = AudioManager.instance.CreateInstance(FMODEvents.instance.overheatedSound);
    }

    private void OnEnable()
    {
        gun.OnBulletShot += BuylletFired;
    }
    private void OnDisable()
    {
        gun.OnBulletShot -= BuylletFired;
    }

    private void Update()
    {
        preOverheatSound.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position + offset));
        overheatedSound.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position + offset));

        if (Input.GetKeyDown(KeyCode.R) && !isGunOverheated) {

            bulletType = EnumExtensions.GetNextEnumValue(bulletType);

            gaugeChange = (gaugeChange == 0f) ? 1f : 0f;

            gaugeSound.setParameterByName("GaugeMode", gaugeChange);

            gaugeSound.start();

        }

        Debug.Log("Volumne: " + overheatingVol);
    }

    private void BuylletFired()
    {
        switch (bulletType) {
            case BulletType.Age:
                if (currentBulletsFired <= maxBulletsFired) {
                    currentBulletsFired++;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    HandleOverheatSoundPositive();

                    if (currentBulletsFired == maxBulletsFired) {
                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;

            case BulletType.Deage:
                if (currentBulletsFired >= -maxBulletsFired) {
                    currentBulletsFired--;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    HandleOverheatSoundNegative();

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

    IEnumerator OverheatCooldown(int seconds)
    {
        gun.ChangeOverheated(true);
        isGunOverheated = true;

        currentBulletsFired = 0;
        OnBulletFired?.Invoke(currentBulletsFired, seconds);

        yield return new WaitForSeconds(seconds);

        gun.ChangeOverheated(false);
        isGunOverheated = false;
    }


    private void HandleOverheatSoundPositive()
    {
        float value = overheatingVol;

        PLAYBACK_STATE overheatPlaybackState;
        preOverheatSound.getPlaybackState(out overheatPlaybackState);

        if (currentBulletsFired == warningBulletsFired) {
            preOverheatSound.start();
            preOverheatSound.setParameterByName("Heat", 3);
        }

        if (currentBulletsFired < warningBulletsFired && overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            preOverheatSound.setParameterByName("Heat", 0);
        }

        if (currentBulletsFired == maxBulletsFired) {
            StartCoroutine(LerpVolume(1f, 1.0f));
            overheatedSound.start();
        }
    }

    private void HandleOverheatSoundNegative()
    {
        float value = overheatingVol;

        PLAYBACK_STATE overheatPlaybackState;
        preOverheatSound.getPlaybackState(out overheatPlaybackState);

        if (currentBulletsFired == -warningBulletsFired) {
            preOverheatSound.start();
            preOverheatSound.setParameterByName("Heat", 3);
        }

        if (currentBulletsFired > -warningBulletsFired && overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            preOverheatSound.setParameterByName("Heat", 0);
        }

        if (currentBulletsFired == -maxBulletsFired) {
            StartCoroutine(LerpVolume(1f, 1.0f));
            overheatedSound.start();
        }
    }

    public IEnumerator LerpVolume(float timeToTake, float currentVolume)
    {
        while (currentVolume > 0) {
            currentVolume -= (Time.deltaTime / timeToTake);
            preOverheatSound.setVolume(currentVolume);

            yield return new WaitForEndOfFrame();
        }

        preOverheatSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        preOverheatSound.setVolume(1);

        yield return null;
    }

    private EventInstance gaugeSound;
    private EventInstance preOverheatSound;
    private EventInstance overheatedSound;
}
