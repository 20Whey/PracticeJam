using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class uiElements : MonoBehaviour
{
    public GameObject enemyManagerScript;
    public GameObject playerCharacterScript;

    public TextMeshProUGUI waveTimer;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI health;
    public int waveNum;
    public int healthNum;
    float currentTime;

    //public Text highScore;

    void Update()
    {
        //Timer
        currentTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //Wave Counter
        waveNum = enemyManagerScript.GetComponent<EnemyManager>().waveCount;
        waveTimer.text = waveNum.ToString();

        //Health
        healthNum = playerCharacterScript.GetComponent<PlayerMovementScript>().currentHealth;
        health.text = healthNum.ToString();

        
    }

}
