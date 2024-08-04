using FMOD.Studio;
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

    float overheatingVol;

    public int MaxBulletsFired => maxBulletsFired;

    private void Awake()
    {
        gun = GetComponent<GunScript>();
        bulletType = BulletType.Age;

        currentBulletsFired = 0;

        gaugeSound = AudioManager.instance.CreateInstance(FMODEvents.instance.gaugeSound);
        overheatSound = AudioManager.instance.CreateInstance(FMODEvents.instance.overheatingSound);
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
        overheatSound.getPlaybackState(out overheatPlaybackState);

        if (currentBulletsFired == warningBulletsFired) {
            overheatSound.start();
            overheatSound.setParameterByName("Heat", 3);
        }

        if (currentBulletsFired < warningBulletsFired && overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            overheatSound.setParameterByName("Heat", 0);
        }

        if (currentBulletsFired == maxBulletsFired) {
            overheatSound.stop(STOP_MODE.ALLOWFADEOUT);
            overheatedSound.start();
        }
    }

    private void HandleOverheatSoundNegative()
    {
        float value = overheatingVol;

        PLAYBACK_STATE overheatPlaybackState;
        overheatSound.getPlaybackState(out overheatPlaybackState);

        if (currentBulletsFired == -warningBulletsFired) {
            overheatSound.start();
            overheatSound.setParameterByName("Heat", 3);
        }

        if (currentBulletsFired > -warningBulletsFired && overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            overheatSound.setParameterByName("Heat", 0);
        }

        if (currentBulletsFired == -maxBulletsFired) {
            overheatSound.stop(STOP_MODE.ALLOWFADEOUT);
            overheatedSound.start();
        }
    }

    /*public IEnumerator LerpVolume(float timeToTake, float currentVolume)
    {
        //float elapsedTime = 0f;
        float value = currentVolume;
        while (value > 0) {
            currentVolume = Mathf.Lerp(currentVolume, 0.0f, Time.deltaTime / timeToTake);
            overheatSound.setParameterByName("Volume", currentVolume);
            value -= Time.deltaTime / timeToTake;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }*/

    private EventInstance gaugeSound;
    private EventInstance overheatSound;
    private EventInstance overheatedSound;
}
