using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class handleEnemies : MonoBehaviour
{
    // Start is called before the first frame update
List<GameObject> lsOfEnemies;
public float countdown;

void Update(){
GameObject[] tmp = GameObject.FindGameObjectsWithTag("Enemy");
lsOfEnemies = (tmp.ToList()).Distinct().ToList();


    foreach(GameObject go in lsOfEnemies){
    if (go.GetComponent<handleSelf>().isPerformingAction == false || go.GetComponent<handleSelf>().countdown == 0)
    {
    TakeAction(go);
    go.GetComponent<handleSelf>().isPerformingAction = true;
    Debug.Log(go.GetComponent<handleSelf>().lastState);

    }
}



    }






public void TakeAction(GameObject current){
//current
var enC = current.GetComponent<EnemyClass>();
var hndlSelf = current.GetComponent<handleSelf>();
switch(enC.enemyAge){
case EnemyClass.EnemyAge.Young:
hndlSelf.HandleCurrentState("Rush");
hndlSelf.countdown = 2.5f;

break;
case EnemyClass.EnemyAge.Old:
Debug.Log("triggering");
if(Vector3.Distance(current.transform.position, hndlSelf.pl.position) < 3f){
Debug.Log("a");
hndlSelf.HandleCurrentState("Flee");
} else {
//Cover Check
hndlSelf.HandleCurrentState("SCover");
}
hndlSelf.countdown = 4f;

break;
case EnemyClass.EnemyAge.Prime:
if (Vector3.Distance(current.transform.position, hndlSelf.pl.position) < 4f){
    hndlSelf.HandleCurrentState("Rush");
} else {
hndlSelf.HandleCurrentState("SCover");
}
hndlSelf.countdown = 3f;

break;
}



}


}

