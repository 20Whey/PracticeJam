using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
    using UnityEngine.AI;

public class handleSelf : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform target;
    public Transform pl;
    public bool isPerformingAction = false;
    public float countdown;
    // Start is called before the first frame update
    void Start()
    {
    
    agent = gameObject.GetComponent<NavMeshAgent>(); 
    countdown = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
{
    countdown -= Time.deltaTime;
}
else
{
    Debug.Log("Time has run out!");
    HandleCurrentState("SCover");
    countdown = 4f;

}
    pl = GameObject.Find("PlayerCharacter").transform;


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


    private void HandleCurrentState(string State){
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
            break;
    }}
    }
   // HandleCurrentState("Wander");
    break;
    case "Wander":
    isPerformingAction = true;
    Vector3 point = (Random.insideUnitSphere * 5 ) + gameObject.transform.position;
    agent.destination = point;
    //HandleCurrentState("SCover");
    break;

    case "Rush":    
    isPerformingAction = true;
    agent.destination = pl.position; 
    break;
    


    }


    }


}
