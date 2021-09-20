using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score instance;

    private static int score;

    Text scoreText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        scoreText = GetComponent<Text>();
        scoreText.text = score.ToString("000");
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString("000");
    }

    public void ResetPoints()
    {
        score = 0;
        scoreText.text = score.ToString("000");
    }
}
