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

 if (countdown > 0)
    {
    countdown -= Time.deltaTime;
    }
    else
    {
    Debug.Log("Time has run out!");
    foreach(GameObject go in lsOfEnemies){
    if (go.GetComponent<handleSelf>().isPerformingAction != true)
    {
    TakeAction(go);
    Debug.Log(go.GetComponent<handleSelf>().lastState);

    }
}
    countdown = 4f;



    }


}



public void TakeAction(GameObject current){
//current
var enC = current.GetComponent<EnemyClass>();
var hndlSelf = current.GetComponent<handleSelf>();
switch(enC.enemyAge){
case EnemyClass.EnemyAge.Young:


break;
case EnemyClass.EnemyAge.Old:
if(Vector3.Distance(transform.position, hndlSelf.pl.position) < 3f){
hndlSelf.HandleCurrentState("Flee");
} else {
//Cover Check
hndlSelf.HandleCurrentState("SCover");
}



break;
case EnemyClass.EnemyAge.Prime:

break;
}



}


}

