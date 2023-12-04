using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab, trophyPrefab, poisonPrefab;
    private Vector3 spawnPos1 = new Vector3(25, 0, 0); //instead of new Vector3... you could do Vector3.right (which is 1,0,0) * 25
    private Vector3 spawnPos2 = new Vector3(25, 3, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    public bool gameOver { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnTrophy", startDelay, repeatRate);
    }
   
    // Update is called once per frame
    private void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, spawnPos1, obstaclePrefab.transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        } 
    }

    private void SpawnTrophy()
    {
        //Does the same as Spawn obstacle, but for trophies.
        Instantiate(trophyPrefab, spawnPos2, trophyPrefab.transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        }
    }
}

