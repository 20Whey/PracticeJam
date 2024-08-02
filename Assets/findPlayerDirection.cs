using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findPlayerDirection : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask layer;
    private Transform Player;
    
    void Awake()
    {
    Player = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
   void Update () {
   if(Physics.Raycast(gameObject.transform.position, Player.position, 50f, layer)){
    gameObject.transform.GetChild(0).gameObject.SetActive(true);
    } else {
    gameObject.transform.GetChild(0).gameObject.SetActive(false);  
        }
    }
}
