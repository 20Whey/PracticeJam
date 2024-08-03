using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
    using UnityEngine.AI;

public class handleSelf : MonoBehaviour
{
    public string lastState;
    private NavMeshAgent agent;

    public GameObject? chosenCoverPoint;
    public Transform target;
    public Transform pl;
    public bool isPerformingAction = false;
    public float countdown;
    public float? coverCountDown;
    // Start is called before the first frame update
    void Start()
    {
    chosenCoverPoint = null;
    agent = gameObject.GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {


    pl = GameObject.Find("PlayerCharacter").transform;

    if(coverCountDown != null) if (coverCountDown > 0){
    coverCountDown -= Time.deltaTime;
    } else {
    coverCountDown = null;
    chosenCoverPoint.GetComponent<Slot>().currentUser = null;
    chosenCoverPoint = null;
    isPerformingAction = false;
    }

       // if(Vector3.Distance(agent.transform.position, target.position) < 0.2f) isPerformingAction = false;

    }


    public List<GameObject> findSortedBarriers(){
    List<GameObject> toSort = GameObject.FindGameObjectsWithTag("Barr").ToList();
        return toSort.OrderBy(Barrier => Vector3.Distance(gameObject.transform.position, Barrier.transform.position)).ToList();
    }
   private List<Transform> getAllChilderen(GameObject parent, int num){
    List<Transform> lsOfChild = new List<Transform>();
   for(int i = 0; i < num; i++){
    lsOfChild.Add(parent.transform.GetChild(i));
   // Debug.Log(parent.transform.GetChild(i));
   }
    return lsOfChild.OrderBy(child => Vector3.Distance(gameObject.transform.position, child.position)).ToList();
}


    public void HandleCurrentState(string State){
    switch(State){
   case "SCover": 
   isPerformingAction = true;
   List<GameObject> nearbyBarriers = findSortedBarriers();
   for(var i = 0; i < nearbyBarriers.Count; i++){
    List<Transform> getChildren = getAllChilderen(nearbyBarriers[i], 4);
    foreach(Transform child in getChildren){
       // Debug.Log(child.gameObject.name);
    GameObject Infant = child.GetChild(0).gameObject;
        if (Infant.activeSelf != false && child.GetComponent<Slot>().currentUser == null){
            //MoveToBarrier and start taking cover
            agent.destination = Infant.transform.position;
            child.GetComponent<Slot>().currentUser = gameObject;
            chosenCoverPoint = child.gameObject;
            lastState = "SCover";
            coverCountDown = 5f;

            return;
    }}
    }
    HandleCurrentState("Wander");
    break;
    case "Wander":
    isPerformingAction = false;
    Vector3 point = (Random.insideUnitSphere * 5 ) + gameObject.transform.position;
    agent.destination = point;
    //HandleCurrentState("SCover");
    lastState = "Wander";
    break;

    case "Rush":    
    isPerformingAction = true;
    agent.destination = pl.position; 
    if(lastState == "SCover"){
    


    }
    lastState = "Rush";
    
    break;
    
    case "Flee":
    isPerformingAction = true;
    //tmp
    agent.destination = -pl.position;
    lastState = "Flee";
    break;


    }


    }


}
