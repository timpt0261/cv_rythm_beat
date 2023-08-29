using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMenu : MonoBehaviour
{

    [SerializeField]
    private TMPro.TextMeshProUGUI score;

    [SerializeField]
    private ScoreManager ScoreManager;

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

    private void Update()
    {
        score.text = "Score: " +  ScoreManager.scoreText.text;
    }
}
