using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private BoxCollider playerCollider;
    private bool isSliding = false;
    private float currentPoints = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        isOnGround = true;
        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerCollider = GetComponent<BoxCollider>();
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

    private void OnSliding(InputValue input)
    {
        if (isOnGround && !GameManager.gameOver && !isSliding)
        {
            StartCoroutine("Slide");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        // Store the original collider size and center
        float originalSizeY = playerCollider.size.y;
        float originalCenterY = playerCollider.center.y;

        playerAnimation.SetBool("Crouch_b", true);
        transform.Rotate(Vector3.left * 60);
        playerAnimation.SetFloat("Speed_f", 0.0f);

        // Adjust collider height and center when crouching
        playerCollider.size = new Vector3(playerCollider.size.x, originalSizeY * 0.5f, playerCollider.size.z);
        playerCollider.center = new Vector3(playerCollider.center.x, originalCenterY * 0.5f, playerCollider.center.z);

        yield return new WaitForSeconds(1.0f);

        playerAnimation.SetBool("Crouch_b", false);
        transform.Rotate(Vector3.right * 60);
        playerAnimation.SetFloat("Speed_f", 1.0f);

        // Reset collider height and center when standing up
        playerCollider.size = new Vector3(playerCollider.size.x, originalSizeY, playerCollider.size.z);
        playerCollider.center = new Vector3(playerCollider.center.x, originalCenterY, playerCollider.center.z);
        isSliding = false;
    }

    // Detects whether the players rigibody is on the ground and if it is the player can jump
    private void OnCollisionEnter(Collision collision)
    {
        //makes the currentPoint the amout point to add to the score
        //help i dont know what im doing - seamus
     

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
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 20.0f);
            StartCoroutine("RestartScene");
        }
    }

    private IEnumerator RestartScene()
    {
        Debug.Log("Restarting scene...");
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Starter_Scene");
    }

    //This checks if the gameObject the player collides with has the obstacles tag and if so,
    //ends the game
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trophy") || other.gameObject.CompareTag("Scoreable") || other.gameObject.CompareTag("Poison"))
        {
            Score points = other.gameObject.GetComponent<Score>();
            currentPoints = points.points;
            Debug.Log("works");
        }
        if (other.gameObject.CompareTag("Scoreable"))

        {
            GameManager.ChangeScore((int) currentPoints);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Trophy"))
        {
            GameManager.ChangeScore((int) currentPoints);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Poison"))
        {
            GameManager.ChangeScore((int) currentPoints);
            Destroy(other.gameObject);
        }
    }
}
