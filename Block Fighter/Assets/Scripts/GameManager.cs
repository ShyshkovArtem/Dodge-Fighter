using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject block;
    public float max_X;
    public Transform spawnPoint;
    public float spawmRate;

    bool gameStarted = false;

    public GameObject tapText;
    public TextMeshProUGUI scoreText;

    int score = 0;

    void Update()
    {
        //Later change to menu Start button
        if (Input.GetMouseButtonDown(0) && !gameStarted) 
        { 
            StartSpawning();
            gameStarted = true;
            tapText.SetActive(false);
        }
        
    }

    void StartSpawning () 
    {
        InvokeRepeating("SpawnBlock", 0.5f, spawmRate);
    }

    private void SpawnBlock()
    {
        Vector3 spawnPos = spawnPoint.position;

        spawnPos.x = Random.Range(-max_X, max_X);

        Instantiate(block, spawnPos, Quaternion.identity);

        score++;

        scoreText.text = score.ToString();
    }
}
