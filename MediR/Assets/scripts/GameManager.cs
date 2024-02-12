using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;



public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public AudioManager audioManager;

    public Transform head;
    public GameObject rightHand;
    public GameObject leftHand;

    //visuelle Volumes, die das Kamerabild beeinflussen (Graues Bild, Buntes/Leuchtendes Bild)
    public Volume greyVolume;
    public Volume highVolume;
    public Volume defaultVolume;

    //Bewegendes Objekt, passend zum Atemrythmus.
    public GameObject floatingObject;
    public Collider gameArea;
    public GameObject stopArea;

    //Referenzhöhe des stehenden Spielenden
    public float refHeight;

    //Bei wie viel Prozent der Referenzhöhe zählt man als sitzend
    public float heightThreshold = 0.5f;
    public bool gameStarted = false;
    bool meditaionStarted = false;
    bool loopStarted = false;
    public bool goodMeditation = false;
    //static float t = 0.0f;
    public float speed = 0.5f;
    public float timer = 0.0f;
    Vector3 floaterStartingPos = new Vector3(0,0,0);
    float pulseHeight = 0.0f;

    [Range(0,3)]
    public float performance = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        floaterStartingPos = floatingObject.transform.position;
        refHeight = head.position.y;    
    }

    // Update is called once per frame
    void Update()
    {
        if(head.position.y < (refHeight*heightThreshold) && !gameStarted && gameArea.bounds.Contains(head.position)){
            StartGame();
        }
        if(gameStarted){

            if(meditaionStarted){
                //wenn das spiel läuft müssen die visuals der performance stetig aktualisiert werden.
                UpdatePerformanceResponse(performance);
                CalcPulseHeight();


                floatingObject.transform.position = new Vector3(floaterStartingPos.x, floaterStartingPos.y + pulseHeight, floaterStartingPos.z);
                if(pulseHeight < -0.99){
                    if(!loopStarted){
                        loopStarted = true;
                        if(CorrectPosture(false)){
                            goodMeditation = true;
                        }else{
                            goodMeditation = false;
                        }
                        //Sound für neuen Atemzyklus abspielen
                        audioManager.Play("pulse");
                    }
                }else if(pulseHeight > 0.99){
                    if(loopStarted){
                        loopStarted = false;
                        if(CorrectPosture(true)){
                            goodMeditation = true; 
                        }else{
                            goodMeditation = false;
                        }
                    }
                }
                UpdatePerformance();
                if(timer > 180){
                    EndGame();
                }
            }
        }
        
    }

    public void StartGame(){
        Debug.Log("Game has started");
        stopArea.transform.position = new Vector3(stopArea.transform.position.x, refHeight, stopArea.transform.position.z);
        //audioManager.Play("background-music");
        gameStarted = true;

        
        //Wenn ein Intro gespielt werden soll, wartet die Meditation, bis das Intro fertig abgespielt ist.
        float timeToStart = audioManager.GetDuration("pulse");
        Debug.Log("Zeit für das Intro: " + timeToStart);
        StartCoroutine(StartMeditation(timeToStart));   
        audioManager.Play("intro");

    }

    public void EndGame(){
        Debug.Log("Game has ended");
        audioManager.Play("outro");
        //reset all metrics
        timer = 0.0f;
        performance = 1.5f;
        UpdatePerformanceResponse(performance);
        meditaionStarted = false;
        floatingObject.transform.position = floaterStartingPos;
    }

    public void SetGameStarted(bool b){
        gameStarted = b;
    }

    //Ändert die Reaktion des Spiels auf gute oder schlechte Meditation 
    //float performance nimmt alle Werte von 0.0f bis 3.0f an. 3.0f ist perfektes meditieren. 1.5f ist okay, 0.0f ist nicht gut
    public void UpdatePerformanceResponse(float performance){
        
        //Setze die In-Camera Effekte basierend auf Performance
        if(performance>=2.0f){
            highVolume.weight = performance-2.0f;
        }else if(performance>=1.0f){
            if(performance>=1.5f){
                defaultVolume.weight = (performance-1.0f)*2;
            }else{
                defaultVolume.weight = 1.0f - (performance-1.0f)*2;
            }
        }
        else{
            greyVolume.weight = 1f-performance;
        }
        
    }

    public void UpdatePerformance(){
        if(goodMeditation){
            performance += 0.00015f;
        }
        else{
            performance -= 0.00015f;
        }
    }

    public bool CorrectPosture(bool handUp){
        Debug.Log("Check for correct Posture");
        if(handUp){
            if((rightHand.transform.position.y > head.position.y) && (leftHand.transform.position.y > head.position.y)){
                return true;
            }
            else{
                return false;
            }
        }else{
            if((rightHand.transform.position.y < head.position.y) && (leftHand.transform.position.y < head.position.y)){
                return true;
            }
            else{
                return false;
            }
        }
    }

    //Das Spiel basiert auf Rythmus des Atmens
    //CalcPulseHeight berechnet Sinus basierend auf Zeit der Meditation und Geschwindigkeit
    // von -1 bis 1
    // bei speed von 0.5 dauert ein Puls 4 x Pi also 12,5663706 Sekunden.
    public void CalcPulseHeight(){
        timer += Time.deltaTime;
        pulseHeight = Mathf.Sin(timer * speed);
    }
    
    IEnumerator StartMeditation(float time)
    {
        yield return new WaitForSeconds(time);
        meditaionStarted = true;
        Debug.Log(meditaionStarted);
    }
}
