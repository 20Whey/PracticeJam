using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Jump SFX")]
    [field: SerializeField] public EventReference jumpSound { get; private set; }

    [field: Header("Running SFX")]
    [field: SerializeField] public EventReference runSound { get; private set; }

    [field: Header("Walking SFX")]
    [field: SerializeField] public EventReference walkSound { get; private set; }
    [field: Header("Idle SFX")]
    [field: SerializeField] public EventReference idleSound { get; private set; }

    [field: Header("HitMarker SFX")]
    [field: SerializeField] public EventReference hitMarkerSound { get; private set; }

    [field: Header("Bullet SFX")]
    [field: SerializeField] public EventReference bulletSound { get; private set; }

    [field: Header("Gauge SFX")]
    [field: SerializeField] public EventReference gaugeSound { get; private set; }

    [field: Header("Overheating SFX")]
    [field: SerializeField] public EventReference overheatingSound { get; private set; }

    [field: Header("Overheated SFX")]
    [field: SerializeField] public EventReference overheatedSound { get; private set; }



    public static FMODEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("More than 1 Audio Manager");
        }

        instance = this;
    }
}
