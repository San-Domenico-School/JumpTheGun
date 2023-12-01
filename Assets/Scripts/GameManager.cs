using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreboardText, timeRemainingText;
    [SerializeField] private GameObject toggleGroup, startButton, spawnManager;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem dirtSplatter;
    public static bool gameOver = true;
    private static float score;
    private AudioSource audioSource;
    private int timeRemaining = 60;
    private bool timedGame;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        DisplayUI();
        EndGame();
    }

    private void DisplayUI()
    {
        scoreboardText.text = "Score: " + Mathf.RoundToInt(score).ToString();
        if (timedGame)
        {
            if (timeRemaining > 0)
            {
                timeRemainingText.text = timeRemaining.ToString();
            }
            else
            {
                timeRemainingText.text = "Game\nOver";
            }
        }
    }

    private void TimeCountdown()
    {
        if (timedGame)
        {
            timeRemaining--;
            if (timeRemaining <= 0f)
            {
                gameOver = true;
            }
        }
    }

    public void StartGame()
    {
        audioSource.Play();
        toggleGroup.SetActive(false);
        startButton.SetActive(false);
        if (timedGame == true)
        {
            timeRemainingText.gameObject.SetActive(true);
            InvokeRepeating("TimeCountdown", 1f, 1f);
            
        }
        gameOver = false;
        spawnManager.SetActive(true);
        playerAnimator.SetFloat("Speed_f", 1.0f);
        playerAnimator.SetBool("BeginGame_b", true);
        dirtSplatter.Play();
    }

    private void EndGame()
    {
        if (gameOver || timeRemaining == 0)
        {
            gameOver = true;
            playerAnimator.SetBool("BeginGame_b", false);
            playerAnimator.SetFloat("Speed_f", 0f);
            audioSource.Stop();
            CancelInvoke();
        }
    }

    public void SetTimed(bool timed)
    {
        timedGame = timed;
    }

    public static void ChangeScore(int change)
    {
        score += change;
    }



}
