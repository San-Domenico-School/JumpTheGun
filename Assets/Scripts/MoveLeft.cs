using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 20;
    private float leftBound = -15;

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.tag == "Obstacles")
        {
            Destroy(gameObject);
        }
    }
    
}
