using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject block;
    public GameObject coin;
    public float max_X;
    public Transform spawnPoint;
    public float spawmRate;

    public GameObject open_Door;

    bool gameStarted = false;

    public GameObject tapText;
    public TextMeshProUGUI scoreText;

    int score = 0;
    public int coinLvl;
    public TextMeshProUGUI coinLVL_Text;

    void Update()
    {
        //Later change to menu Start button
        if (Input.GetMouseButtonDown(0) && !gameStarted) 
        { 
            StartSpawning();
            gameStarted = true;
            tapText.SetActive(false);
        }

    coinLVL_Text.text = coinLvl.ToString();

    }

    void StartSpawning () 
    {
        InvokeRepeating("SpawnBlock", 0.5f, spawmRate);
    }

    private void SpawnBlock()
    {
        if (score < 10)
        {
            Vector3 spawnPos = spawnPoint.position;

            //for block
            spawnPos.x = Random.Range(-8.1f, 2.7f);

            Instantiate(block, spawnPos, Quaternion.identity);

            score++;

            scoreText.text = score.ToString();

            //for coin
            spawnPos.x = Random.Range(-8.1f, 2.7f);

            Instantiate(coin, spawnPos, Quaternion.identity);
        } else
        {
            open_Door.SetActive(true);
        }
        
    }
}
