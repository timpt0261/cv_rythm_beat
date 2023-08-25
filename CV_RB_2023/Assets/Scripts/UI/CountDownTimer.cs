using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{

    public SongManager songManager;
    public AudioSource countdownSFX;
    float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = songManager.songDelayInSeconds;
        countdownSFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0) 
        {
            timeRemaining -= Time.deltaTime;
        }
        else {
            
        }

    }
}
