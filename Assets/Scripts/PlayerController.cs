using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))] // Ensure AudioSource is present
public class PlayerController : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 10f;
    public float gravity = 110f;
    public float jumpTime = 0.3f; // Maximum duration for holding the jump
    private float jumpTimeCounter;
    private bool isJumping;

    private bool isTouching; // To track touch input

    // Game Over Audio
    private AudioSource audioSource;
    public AudioClip gameOverClip;
    public AudioClip playerJumpClip;  // Jump sound clip

    private void Awake()
    {
        character = GetComponent<CharacterController>();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false; // Ensure it doesn't auto-play
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        isJumping = false;
        jumpTimeCounter = 0f;
        isTouching = false;
    }

    private void Update()
    {
        // Apply gravity
        direction += gravity * Time.deltaTime * Vector3.down;

        // Detect input: Key on desktop, touch on mobile
        bool inputDown = Input.GetKeyDown(KeyCode.Space) || IsTouchDown();
        bool inputHold = Input.GetKey(KeyCode.Space) || IsTouchHeld();
        bool inputUp = Input.GetKeyUp(KeyCode.Space) || IsTouchUp();

        if (character.isGrounded) // Check if the player is grounded
        {
            direction = Vector3.down; // Reset downward velocity when grounded

            if (inputDown) // Start jumping
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                direction = Vector3.up * jumpForce; // Apply jump force

                // Play jump sound if it's not already playing
                if (playerJumpClip != null && !audioSource.isPlaying)
                {
                    audioSource.clip = playerJumpClip;
                    audioSource.loop = false;
                    audioSource.Play(); // Play the jump sound
                }
            }
        }

        // Handle jump hold
        if (inputHold && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                direction = Vector3.up * jumpForce; // Continue applying jump force
                jumpTimeCounter -= Time.deltaTime; // Decrease jump timer
            }
            else
            {
                isJumping = false; // Stop jumping if timer runs out
            }
        }

        // Stop jumping when the button is released
        if (inputUp)
        {
            isJumping = false;
        }

        // Move the player
        character.Move(direction * Time.deltaTime);
    }

    // Handle collision with obstacles
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {    
            // Play Game Over sound immediately
            if (gameOverClip != null)
            {
                audioSource.clip = gameOverClip;
                audioSource.loop = false;
                audioSource.Play(); // Play immediately
            }

            ClassicModeManager.Instance.GameOver();
        }
    }

    // Helper methods for touch detection
    private bool IsTouchDown()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                return true;
            }
        }
        return false;
    }

    private bool IsTouchHeld()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                isTouching = true;
                return true;
            }
        }
        return false;
    }

    private bool IsTouchUp()
    {
        if (isTouching && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                return true;
            }
        }
        return false;
    }
}
