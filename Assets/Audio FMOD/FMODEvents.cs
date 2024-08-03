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



    public static FMODEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("More than 1 Audio Manager");
        }

        instance = this;
    }
}
