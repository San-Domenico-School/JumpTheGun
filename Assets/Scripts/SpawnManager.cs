using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This script is attached to the Spawn Manager
 * */

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefab, trophyPrefab, poisonPrefab;
    private Vector3 spawnPos1 = new Vector3(25, 0, -0.26f); //instead of new Vector3... you could do Vector3.right (which is 1,0,0) * 25
    private Vector3 spawnPos2 = new Vector3(25, 5, -0.26f);
    private Vector3 spawnPos3 = new Vector3(22, 5, -0.26f);
    private float startDelay = 2;
    public bool gameOver { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, 4);
        InvokeRepeating("SpawnTrophy", startDelay, 2);
        InvokeRepeating("SpawnPoison", startDelay, 6);
    }
   
    // Update is called once per frame
    private void SpawnObstacle()
    {
        int index = UnityEngine.Random.Range(0, obstaclePrefab.Length);
        Instantiate(obstaclePrefab[index], spawnPos1, obstaclePrefab[index].transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        } 
    }

    private void SpawnTrophy()
    {
        float yPos = UnityEngine.Random.Range(2.0f, 6.2f);
        spawnPos2 = new Vector3(spawnPos2.x, yPos, spawnPos2.z);
        //Does the same as Spawn obstacle, but for trophies.
        int index = UnityEngine.Random.Range(0, trophyPrefab.Length);
        Instantiate(trophyPrefab[index], spawnPos2, trophyPrefab[index].transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        }
    }

    private void SpawnPoison()
    {
        float yPos = UnityEngine.Random.Range(2.0f, 6.2f);
        spawnPos3 = new Vector3(spawnPos3.x, yPos, spawnPos3.z);
        //Does the same as Spawn obstacle, but for poisons.
        int index = UnityEngine.Random.Range(0, poisonPrefab.Length);
        Instantiate(poisonPrefab[index], spawnPos3, poisonPrefab[index].transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        }
    }
}

