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
    countdown = 4f;
    chosenCoverPoint = null;
    agent = gameObject.transform.parent.GetComponent<NavMeshAgent>(); 

    pl = GameObject.Find("PlayerCharacter").transform;

    }

	// Update is called once per frame
	void Update()
	{
		if (countdown > 0)
		{
			countdown -= Time.deltaTime;
			//  isPerformingAction = false;
		}
		else
		{
			isPerformingAction = false;

		}



		if (coverCountDown != null) if (coverCountDown > 0)
			{
				coverCountDown -= Time.deltaTime;
			}
			else
			{
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
   private List<Transform> getAllChilderen(GameObject parent, int possiblePoints){
    List<Transform> lsOfChild = new List<Transform>();
    for(var i = 0; i < parent.transform.childCount; i++){
    if (parent.transform.GetChild(i) != null){
        lsOfChild.Add(parent.transform.GetChild(i));
    }
    
    }

    return lsOfChild.OrderBy(child => Vector3.Distance(gameObject.transform.position, child.position)).ToList();
}


	public void HandleCurrentState(string State)
	{
		switch (State)
		{
			case "SCover":
				isPerformingAction = true;
				//agent.destination = transform.position;
				List<GameObject> nearbyBarriers = findSortedBarriers();
				for (var i = 0; i < nearbyBarriers.Count; i++)
				{
					List<Transform> getChildren = getAllChilderen(nearbyBarriers[i], 4);
					foreach (Transform child in getChildren)
					{
						// Debug.Log(child.gameObject.name);
						GameObject Infant = child.GetChild(0).gameObject;
						if (Infant.activeSelf != false && child.GetComponent<Slot>().currentUser == null)
						{
							//MoveToBarrier and start taking cover
							agent.destination = Infant.transform.position;
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
				isPerformingAction = true;
				Vector3 point = (Random.insideUnitSphere * 5) + gameObject.transform.position;
				agent.destination = point;
				//HandleCurrentState("SCover");
				lastState = "Wander";
				break;

			case "Rush":
				isPerformingAction = true;
				agent.destination = pl.position;
				if (lastState == "SCover")
				{
				}
				lastState = "Rush";

				break;

			case "Flee":
				isPerformingAction = true;
				//get where I'm pointing and move the opposing way

				Vector3 dtp = transform.position - pl.position;
				Vector3 np = gameObject.transform.position + dtp;    //Vector3.Distance(transform.position, pl.position);


				agent.destination = np + new Vector3(np.x, 0, np.z);
				lastState = "Flee";
				break;


		}


	}


}
