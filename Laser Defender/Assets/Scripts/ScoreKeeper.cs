using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public static int score = 0;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        Reset();
        text.text = ("Score: " + 0.ToString());
    }

    public void Score(int points)
    {
        score += points;
        text.text = ("Score: " + score);
    }

    public static void Reset()
    {
        score = 0;
    }
}
