using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*********************************************
 * Th goal of the PLayer Controller is to allow
 * the player to move and jump while playing the
 * game. it detects when the player's rigidbody is
 * on the ground and if it is, it allows the player
 * to jump.
 * 
 * Logan Rudsenske
 * Player Controller Version 1.0
 ********************************************/


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce, gravityModifier;
    [SerializeField] private ParticleSystem explosionParticle, dirtParticle;
    [SerializeField] private AudioClip jumpSound, crashSound;
    private Animator playerAnimation;
    private AudioSource playerAudio;
    private Rigidbody playerRb;
    private bool isOnGround;
    

    // Start is called before the first frame update
    private void Start()
    { 
        isOnGround = true;
        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
        playerAudio  = GetComponent<AudioSource>();

    }

    // If the player is on the ground then this force is applied when the input for jump is pressed
    // and the player jumps the dirt particles will then stop and the jump sound plays
    private void OnJump(InputValue input)
    {
        if (isOnGround && !GameManager.gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimation.SetTrigger("Jump_trig");
            isOnGround = false;
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 10.0f);
        }
    }

    // Detects whether the players rigibody is on the ground and if it is the player can jump
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground" && !GameManager.gameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
            
        }

        //This checks if the gameObject the player collides with has the obstacles tag and if so,
        //ends the game and plays the death animation, and the dirt particles stop and the
        //death sound plays
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            GameManager.gameOver = true;
            playerAnimation.SetBool("Death_b", true);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 20.0f);
        }
    }

    //This checks if the gameObject the player collides with has the obstacles tag and if so,
    //ends the game
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("Scoreable"))
        {
            GameManager.ChangeScore(5);
        }
    }
}
