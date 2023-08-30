using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Interaction;
public class Lane : MonoBehaviour
{
	public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
	public bool detectorIsActive;
	public HandCollisionDetector detector = null;
	public HandTracking tracker = null;
	public GameObject notePrefab;
	List<Note> notes = new List<Note>();
	public List<double> timeStamps = new List<double>();
	
	int spawnIndex = 0;
	int inputIndex = 0;
	
	public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
	{
		foreach (var note in array)
		{
			if (note.NoteName == noteRestriction)
			{
				var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan> (note.Time, SongManager.midiFile.GetTempoMap());
				timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
			}
		}
	}

    private void Start()
    {
		Debug.Log(this.name + " Hand Collison Detector is  null : " + (detector == null).ToString());
		Debug.Log(this.name + " Hand Tracker is  null : " + (tracker == null).ToString());
		

	}

    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
		{
			if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
			{
				var note = Instantiate(notePrefab, transform);
				notes.Add(note.GetComponent<Note>());
				note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
				spawnIndex++;
			}
		}
		
		if (inputIndex < timeStamps.Count)
		{
			double timeStamp = timeStamps[inputIndex];
			double marginOfError = SongManager.Instance.marginOfError;
			double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			var check = false;

			if (detector)
			{
				check = detector.active;
				Debug.Log("Set to detector for " + this.name);

			}
			else if (tracker) 
			{
				check = tracker.img_has_thumb_up;
				Debug.Log("Set to tracker for " + this.name);
			}
			else {
				Debug.LogError("Must have a detector or a tracker!");
				return;

			}

			if (check)
			{
				if (Mathf.Abs((float)audioTime - (float)timeStamp) < marginOfError)
				{
					HandleHit();
					
				}
				else
				{
					//print($"Hit inaccurate on {inputIndex} note with {Mathf.Abs((float)audioTime - (float)timeStamp)} delay");
				}
			}
			

			if (timeStamp + marginOfError <= audioTime)
			{
				HandleMiss();
			}
		}
    }

	private void HandleHit()
	{
		Hit();
		print($"Hit on {inputIndex} note");
		notes[inputIndex].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
		SetEmissiveMaterial(notes[inputIndex].gameObject);
		inputIndex++;

	}

	private void HandleMiss()
	{
		
		Miss();
        print($"Missed {inputIndex} note");
        notes[inputIndex].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        SetEmissiveMaterial(notes[inputIndex].gameObject);
        inputIndex++;
	}

	private void SetEmissiveMaterial(GameObject obj)
	{
		Material material = obj.GetComponent<Renderer>().material;
		Color newColor = new Color(material.color.r, material.color.g, material.color.b, 0.5f); // Adjust the alpha for transparency
		material.SetColor("_Color", newColor);
		material.EnableKeyword("_EMISSION");
		material.SetColor("_EmissionColor", newColor);
	}

	private void Hit()
	{
		ScoreManager.Hit();
	}
	
	private void Miss()
	{
		ScoreManager.Miss();
	}

	

}
