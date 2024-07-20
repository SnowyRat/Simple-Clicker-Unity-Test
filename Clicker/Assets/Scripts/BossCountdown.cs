using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class BossCountdown : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 30f;
    [SerializeField] private TextMeshProUGUI countdownText;
    private bool startTimer = false;
    [SerializeField] private Slider slider;
    

    // Start is called before the first frame update
    void Start()
    {

        currentTime = startingTime;
        slider.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false); // Hide the countdown text initially

    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer){
            currentTime -=Time.deltaTime;
            slider.value = currentTime / startingTime;
            countdownText.text = currentTime.ToString("F2");
            if(currentTime<=0){
                startTimer=false;
                slider.gameObject.SetActive(false);
                countdownText.gameObject.SetActive(false); // Hide the countdown text when the timer ends

            }
        }
    }
    public bool TimerEnded(){
        if(currentTime <=0){
            return true;
        }
        else{
            return false;
        }
    }
    public void StartTimer(){
        startTimer = true;
        slider.value = 1.0f;
        slider.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true); // Show the countdown text when the timer starts

        startingTime = 30f;
        currentTime = startingTime;
    }
    public void EndTimer(){
        startTimer = false;
        slider.value = 0.0f;
        slider.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false); // Hide the countdown text when the timer ends
        startingTime = 30f;
        currentTime = startingTime;
    }
    public float GetCounter(){
        return currentTime;
    }
}
