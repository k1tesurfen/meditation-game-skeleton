using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;



public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public AudioManager audioManager;

    public GameObject player;
    public Volume greyVolume;
    public Volume highVolume;
    public Volume defaultVolume;
    public GameObject ball;

    public float refHeight;
    public float heightThreshold = 0.5f;
    public bool gameStarted = false;
    //static float t = 0.0f;
    public float speed = 0.5f;

    [Range(0,3)]
    public float performance = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        refHeight = player.transform.position.y;    
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < (refHeight*heightThreshold) && !gameStarted){
            StartGame();
        }
        if(gameStarted){
            UpdatePerformanceResponse(performance);
            ball.transform.position = new Vector3(ball.transform.position.x, 1 + Mathf.Sin(speed * Time.time), ball.transform.position.z);
        }
        
    }

    public void StartGame(){
        Debug.Log("Game has started");
        gameStarted = true;
    }

    //Ändert die Reaktion des Spiels auf gute oder schlechte Meditation 
    //float performance nimmt alle Werte von 0.0f bis 3.0f an. 3.0f ist perfektes meditieren. 1.5f ist okay, 0.0f ist nicht gut
    public void UpdatePerformanceResponse(float performance){
        
        //Setze die In-Camera Effekte basierend auf Performance
        //Wenn 
        if(performance>=2.0f){
            highVolume.weight = performance-2.0f;
        }else if(performance>=1.0f){
            if(performance>=1.5f){
                defaultVolume.weight = (performance-1.0f)*2;
            }else{
                defaultVolume.weight = 1.0f - (performance-1.0f)*2    ;
            }
        }
        else{
            greyVolume.weight = 1f-performance ;
        }
        
    }

    public bool CorrectPosture(){
        return true;
    }
}
