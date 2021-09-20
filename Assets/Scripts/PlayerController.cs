using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravityModifier = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;
    [SerializeField] private InputAction newMovementInput;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensivity = 1f;
    [SerializeField] float maxViewAngle = 60f;
    [SerializeField] private Text healthText; 

    private CharacterController characterController;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 heightMovement;

    private bool jump = false;

    private Transform mainCamera;

    private Animator anim;

    private GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if(Camera.main.GetComponent<CameraController>() == null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        mainCamera = GameObject.FindGameObjectWithTag("CameraPoint").transform;
    }
    private void OnEnable()
    {
        newMovementInput.Enable();
    }

    private void OnDisable()
    {
        newMovementInput.Disable();
    }

    void Update()
    {
        if (gameManager.GetLevelFinish)
        {
            newMovementInput.Disable();
        }
        UpdateHealth();

        KeyboardInput();

        AnimationChanger();
    }

    private void FixedUpdate()
    {
         Move();

         Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x,
            transform.eulerAngles.z);
        if(mainCamera != null)
        {
            
            if(mainCamera.eulerAngles.x > maxViewAngle && mainCamera.eulerAngles.x < 180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else if(mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles +
                    new Vector3(-MouseInput().y, 0f, 0f));
            }
        }
    }

    private void Move()
    {
        if (jump)
        {
            heightMovement.y = jumpPower;
            jump = false;
        }

        heightMovement.y -= gravityModifier * Time.deltaTime;

        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;
        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;

        characterController.Move(movementVector + heightMovement);

        if (characterController.isGrounded)
        {
            heightMovement.y = 0f;
        }
    }

    private void KeyboardInput()
    {
        horizontalInput = newMovementInput.ReadValue<Vector2>().x;
        verticalInput = newMovementInput.ReadValue<Vector2>().y;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && characterController.isGrounded)
        {
            jump = true;
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
    private Vector2 MouseInput()
    {
        return new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue()) * mouseSensivity;
    }

    private void AnimationChanger()
    {
        if(newMovementInput.ReadValue<Vector2>().magnitude > 0f && characterController.isGrounded)
        {
            if(currentSpeed >= 12f)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
            }
            else 
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
    }
    private void UpdateHealth()
    {
        healthText.text = "Health: " + gameObject.GetComponent<Health>().GetHealth;
    }
}
