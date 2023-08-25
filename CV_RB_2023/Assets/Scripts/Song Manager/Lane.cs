using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Interaction;
public class Lane : MonoBehaviour
{
	public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
	public bool detectorIsActive;
	public HandCollisionDetector detector;
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

    // Update is called once per frame
    void Update()
    {
		detectorIsActive = detector.active;

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
			
			if (detector.active)
			{
				if (Mathf.Abs((float)audioTime - (float)timeStamp) < marginOfError)
				{
					Hit();
					print($"Hit on {inputIndex} note");
					//Destroy(notes[inputIndex].gameObject);
					notes[inputIndex].gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
					notes[inputIndex].gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
					notes[inputIndex].gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
					inputIndex++;
				}
				else
				{
					print($"Hit inaccurate on {inputIndex} note with {Mathf.Abs((float)audioTime - (float)timeStamp)} delay");
				}
			}
			if (timeStamp + marginOfError <= audioTime)
			{
				Miss();
				print($"Missed {inputIndex} note");
				inputIndex++;
			}
		}
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
