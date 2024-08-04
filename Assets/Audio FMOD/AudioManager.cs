using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private EventInstance musicEventInstance;

    private EventInstance ambienceEventInstance;

    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("More than 1 Audio Manager");
        }

        instance = this;
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
        //InitializeAmbience(FMODEvents.instance.ambience);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        musicEventInstance = CreateInstance(ambienceEventReference);
        musicEventInstance.start();
    }

}
