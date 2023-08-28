using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMenu : MonoBehaviour
{

    [SerializeField]
    private TMPro.TextMeshProUGUI score;

    //Load Scene
    public void Play()
    {
        SceneManager.LoadScene("MainPlatform");
    }

    //Quit Game
    public void Quit()
    {
        Debug.Log("Game quit successfully");
        Application.Quit();
    }

    private void Start()
    {
        // Read JSON from the file
        string json = File.ReadAllText("SaveData.json");

        // Convert JSON back to SaveData
        string saveData = JsonUtility.FromJson<string>(json);

        // Use the loaded data as needed
        score.text = saveData;
    }
}
