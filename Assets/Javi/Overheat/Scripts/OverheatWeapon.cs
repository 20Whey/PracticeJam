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

    public int MaxBulletsFired => maxBulletsFired;

    private void Awake()
    {
        gun = GetComponent<GunScript>();
        bulletType = BulletType.Age;

        currentBulletsFired = 0;

        gaugeSound = AudioManager.instance.CreateInstance(FMODEvents.instance.gaugeSound);
        overheatingSound = AudioManager.instance.CreateInstance(FMODEvents.instance.overheatingSound);
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

        Debug.Log("Gauge Value: " + gaugeChange);
    }

    private void BuylletFired()
    {
        switch (bulletType) {
            case BulletType.Age:
                if (currentBulletsFired <= maxBulletsFired) {
                    currentBulletsFired++;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    HandlePreOverheatSoundPositive();

                    if (currentBulletsFired == maxBulletsFired) {
                        //HandleOverheatSound();

                        OnWeaponOverheated?.Invoke(bulletType);
                        StartCoroutine(OverheatCooldown(overheatCooldownTime));
                    }
                }
                break;

            case BulletType.Deage:
                if (currentBulletsFired >= -maxBulletsFired) {
                    currentBulletsFired--;
                    OnBulletFired?.Invoke(currentBulletsFired, 1f);

                    //HandlePreOverheatSound();

                    if (currentBulletsFired == -maxBulletsFired) {
                        //HandleOverheatSound();

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


    private void HandlePreOverheatSoundPositive()
    {
        float overheatingVol = 1.0f;

        PLAYBACK_STATE overheatPlaybackState;
        overheatingSound.getPlaybackState(out overheatPlaybackState);

        if (currentBulletsFired == warningBulletsFired) {
            overheatingSound.start();
            overheatingSound.setParameterByName("Heat", 3);
        }

        if (currentBulletsFired < warningBulletsFired && overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            overheatingSound.setParameterByName("Heat", 0);
        }

        if (currentBulletsFired == maxBulletsFired) {
            overheatingSound.setParameterByName("Fader", 0);
            overheatedSound.start();
            if (overheatPlaybackState != (PLAYBACK_STATE.STOPPED)) {
                overheatingSound.setVolume(Mathf.Lerp(overheatingVol, 0.0f, Time.deltaTime / 1f));
                if (overheatingVol == 0.0f) {
                    overheatingSound.stop(STOP_MODE.ALLOWFADEOUT);
                }
            }
        }
    }

    private void HandleOverheatSound()
    {
        float overheatVol = 1.0f;

        PLAYBACK_STATE overheatPlaybackState;
        overheatingSound.getPlaybackState(out overheatPlaybackState);

        if (overheatPlaybackState.Equals(PLAYBACK_STATE.PLAYING)) {
            overheatingSound.setVolume(Mathf.Lerp(overheatVol, 0.0f, Time.deltaTime / 1f));
            if (overheatVol == 0.0f) {
                overheatingSound.stop(STOP_MODE.IMMEDIATE);
                overheatingSound.setParameterByName("Heat", 4);
                overheatingSound.start();
            }
        }
    }

    private EventInstance gaugeSound;
    private EventInstance overheatingSound;
    private EventInstance overheatedSound;
}
