using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private void Awake()
    {
        // Get reference to CharacterController
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Reset movement direction when the object is enabled
        direction = Vector3.zero;
    }

    private void Update()
    {
        // Apply gravity to the direction vector
        direction += gravity * Time.deltaTime * Vector3.down;

        // Check for jump input (platform-specific handling)
#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && character.isGrounded)
        {
            direction = Vector3.up * jumpForce; // Apply jump force
        }
#else
        if (Input.GetKey(KeyCode.Space) && character.isGrounded)
        {
            direction = Vector3.up * jumpForce; // Apply jump force
        }
#endif

        // Move the character based on the direction vector
        character.Move(direction * Time.deltaTime);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // Trigger game over if the player collides with an obstacle
    //     if (other.CompareTag("Obstacle"))
    //     {
    //         GameManager.Instance.GameOver();
    //     }
    // }
}
