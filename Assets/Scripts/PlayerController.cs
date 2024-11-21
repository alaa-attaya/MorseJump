using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 10f;
    public float gravity = 50f;
    public float jumpTime = 0.2f; // Maximum duration for holding the jump
    private float jumpTimeCounter;
    private bool isJumping;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        isJumping = false;
        jumpTimeCounter = 0f;
    }

    private void Update()
    {
        // Apply gravity
        direction += gravity * Time.deltaTime * Vector3.down;

        if (character.isGrounded) // Check if the player is grounded
        {
            direction = Vector3.down; // Reset downward velocity when grounded

            if (Input.GetKeyDown(KeyCode.Space)) // Start jumping
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                direction = Vector3.up * jumpForce; // Apply jump force
            }
        }

        // Handle jump hold
        if (Input.GetKey(KeyCode.Space) && isJumping)
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // Move the player
        character.Move(direction * Time.deltaTime);
    }

    // Uncomment and implement the trigger logic for game over if necessary:
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Obstacle")) {
    //         GameManager.Instance.GameOver();
    //     }
    // }
}
