using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
	public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public int inputDelayInMilliseconds;

    public string fileLocation;
    public float noteTime;
    public float noteSpawnZ;
    public float noteTapZ;

    public ScoreManager scoreManager;
    public Canvas scoreMenuCanvas;

    public float noteDespawnZ
    {
        get {
            return noteTapZ - (noteSpawnZ - noteTapZ);
        
        }
    }

    public static MidiFile midiFile;
    // private bool levelStarted = false;

    [SerializeField]
    private ScoreManager scoreManger;
    
    private void Start()
    {
        Instance = this;
        scoreMenuCanvas.enabled = false;
        if (Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else 
        {
           ReadFromFile();
        }

        
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation)) 
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else 
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
        Debug.Log(midiFile);
    }

    public void GetDataFromMidi() 
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);
        Debug.Log(notes);

		foreach (var lane in lanes) lane.SetTimeStamps(array);
		
        Invoke(nameof(StartSong), songDelayInSeconds);
        
    }

    public void  StartSong()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime() 
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }


    private void Update()
    {
        // Check if the audio source is playing and the song has started
        if (audioSource.isPlaying && GetAudioSourceTime() >= songDelayInSeconds)
        {
            Debug.Log("Level is still ongoing.");
        }
        else
        {
            scoreMenuCanvas.enabled = false;
        }
    }

}
