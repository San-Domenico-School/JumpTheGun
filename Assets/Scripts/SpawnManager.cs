using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0); //instead of new Vector3... you could do Vector3.right (which is 1,0,0) * 25
    private float startDelay = 2;
    private float repeatRate = 2;
    public bool gameOver { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }
   
    // Update is called once per frame
    private void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        if (gameOver)
        {
            CancelInvoke();
            GameManager.gameOver = true;
        } 
    }
}
