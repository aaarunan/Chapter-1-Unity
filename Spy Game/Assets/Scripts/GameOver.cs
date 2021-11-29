using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Guard>().OnPlayerFound += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameOver()
    {
        gameOverScreen.SetActive(true);
        //secondsSurvivedUI.text = Mathf.RoundToInt(Time.timeSinceLevelLoad).ToString();
        gameOver = true;
    }
}
