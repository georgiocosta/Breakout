using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    Text livesText;

    void Start()
    {
        livesText = GetComponent<Text>();
    }

    public void SetLives(int lives)
    {
        if (lives >= 0)
        {
            livesText.text = lives.ToString();
        }
    }
}
