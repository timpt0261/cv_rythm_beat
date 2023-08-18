using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Interaction;
public class Lane : MonoBehaviour
{
	public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
	public bool detectorIsActive;
	public GameObject notePrefab;
	List<Note> notes = new List<Note>();
	public List<double> timeStamps = new List<double>();
	
	int spawnIndex = 0;
	int inputIndex = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
	{
		foreach (var note in array)
		{
			if (note.NoteName == noteRestriction)
			{
				var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan> (note.Time, Song_Manager.midiFile.GetTempoMap());
				timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
		{
			if (Song_Manager.GetAudioSourceTime() >= timeStamps[spawnIndex] - Song_Manager.Instance.noteTime)
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
			double marginOfError = Song_Manager.Instance.marginOfError;
			double audioTime = Song_Manager.GetAudioSourceTime() - (Song_Manager.Instance.inputDelayInMilliseconds / 1000.0);
			
			if (detectorIsActive)
			{
				if (Mathf.Abs((float)audioTime - (float)timeStamp) < marginOfError)
				{
					Hit();
					print($"Hit on {inputIndex} note");
					Destroy(notes[inputIndex].gameObject);
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
		Score_Manager.Hit();
	}
	
	private void Miss()
	{
		Score_Manager.Miss();
	}
}
