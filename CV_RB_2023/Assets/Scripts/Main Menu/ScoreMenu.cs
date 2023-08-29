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

    public void Menu() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        score.text = ScoreManager.scoreText.text;
    }
}
