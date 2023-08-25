using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scorebar : MonoBehaviour
{
	public float score = 0;
	public float maxScore = 0;
	private Image scoreBar;
	public static scorebar Instance;
	
    // Start is called before the first frame update
    void Start()
    {
        scoreBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
		score = (float)ScoreManager.comboScore;
		maxScore = (float)ScoreManager.maxScore;
		if (maxScore != 0)
		{
			scoreBar.fillAmount = score / maxScore;
		}
    }
}
