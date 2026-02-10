using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{

    public float roundTime = 300f;
    public TextMeshProUGUI timerText;

    float timeRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = roundTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            timerText.text = "00:00";

            //Posar llogica de final de ronda aquí
        }

        void UpdateTimerDisplay(float timeToDisplay)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }
}
