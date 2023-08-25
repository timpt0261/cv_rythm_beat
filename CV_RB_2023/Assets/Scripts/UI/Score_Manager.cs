using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class Score_Manager : MonoBehaviour
{
	public static Score_Manager Instance;
	public AudioSource hitSFX;
	public AudioSource missSFX;
	public TMPro.TextMeshPro scoreText;
	public static int comboScore;
	//public static MidiFile midiFile;
	public static int maxScore;
	
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
		comboScore = 0;
    }
	
	public static void Hit()
	{
		comboScore += 1;
		maxScore += 1;
		Instance.hitSFX.Play();
		// play hit sfx
	}
	
	public static void Miss()
	{
		//comboScore = 0;
		maxScore += 1;
		Instance.missSFX.Play();
		// play miss sfx
	}

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score : " + comboScore.ToString() + "/" + maxScore;
		//scorebar.score = comboScore;
		//scorebar.maxScore = maxScore;
		
    }
}
