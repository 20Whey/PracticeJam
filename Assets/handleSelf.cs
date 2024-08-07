using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class handleSelf : MonoBehaviour
{
    public string lastState;
    private NavMeshAgent agent;

    public GameObject? chosenCoverPoint;
    public Transform target;
    [SerializeField] public Transform pl;
    public bool isPerformingAction = false;
    public float countdown;
    public float? coverCountDown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = 4f;
        chosenCoverPoint = null;
        agent = gameObject.transform.parent.GetComponent<NavMeshAgent>();
        pl = FindObjectOfType<PlayerMovementScript>().gameObject.transform;

        enemyMovSound = AudioManager.instance.CreateInstance(FMODEvents.instance.enemyMovSound);
        enemyMovSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0) {
            countdown -= Time.deltaTime;
            //  isPerformingAction = false;
        } else {
            isPerformingAction = false;

        }

        enemyMovSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        if (coverCountDown != null) if (coverCountDown > 0) {
                coverCountDown -= Time.deltaTime;
                enemyMovSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            } else {
                coverCountDown = null;
                //TODO: If this is NOT null stop sound
                chosenCoverPoint.GetComponent<Slot>().currentUser = null;
                chosenCoverPoint = null;
                isPerformingAction = false;
            }

        // if(Vector3.Distance(agent.transform.position, target.position) < 0.2f) isPerformingAction = false;

    }


    public List<GameObject> findSortedBarriers()
    {
        List<GameObject> toSort = GameObject.FindGameObjectsWithTag("Barr").ToList();
        return toSort.OrderBy(Barrier => Vector3.Distance(gameObject.transform.position, Barrier.transform.position)).ToList();
    }
    private List<Transform> getAllChilderen(GameObject parent, int num)
    {
        List<Transform> lsOfChild = new List<Transform>();
        for (int i = 0; i < num; i++) {
            lsOfChild.Add(parent.transform.GetChild(i));
            // Debug.Log(parent.transform.GetChild(i));
        }
        return lsOfChild.OrderBy(child => Vector3.Distance(gameObject.transform.position, child.position)).ToList();
    }


    public void HandleCurrentState(string State)
    {
        switch (State) {
            case "SCover":
                isPerformingAction = true;
                //agent.destination = transform.position;
                List<GameObject> nearbyBarriers = findSortedBarriers();
                for (var i = 0; i < nearbyBarriers.Count; i++) {
                    List<Transform> getChildren = getAllChilderen(nearbyBarriers[i], 4);
                    foreach (Transform child in getChildren) {
                        // Debug.Log(child.gameObject.name);
                        GameObject Infant = child.GetChild(0).gameObject;
                        if (Infant.activeSelf != false && child.GetComponent<Slot>().currentUser == null) {
                            //MoveToBarrier and start taking cover
                            agent.destination = Infant.transform.position;
                            enemyMovSound.start();
                            child.GetComponent<Slot>().currentUser = gameObject;
                            chosenCoverPoint = child.gameObject;
                            lastState = "SCover";
                            coverCountDown = 5f;

                            return;
                        }
                    }
                }
                HandleCurrentState("Wander");
                break;
            case "Wander":
                enemyMovSound.start();
                isPerformingAction = true;
                Vector3 point = (Random.insideUnitSphere * 5) + gameObject.transform.position;
                agent.destination = point;
                //HandleCurrentState("SCover");
                lastState = "Wander";
                break;

            case "Rush":
                enemyMovSound.start();
                isPerformingAction = true;
                agent.destination = pl.position;
                if (lastState == "SCover") {
                }
                lastState = "Rush";

                break;

            case "Flee":
                enemyMovSound.start();
                isPerformingAction = true;
                //get where I'm pointing and move the opposing way

                Vector3 dtp = transform.position - pl.position;
                Vector3 np = gameObject.transform.position + dtp;    //Vector3.Distance(transform.position, pl.position);


                agent.destination = np + new Vector3(np.x, 0, np.z);
                lastState = "Flee";
                break;


        }


    }

    private EventInstance enemyMovSound;
}
