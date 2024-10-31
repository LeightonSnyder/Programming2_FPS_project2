using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : MonoBehaviour
{
    //Public floats
    public float WalkSpeed = 5f;
    public float SprintMultiplier = 2f;
    public float JumpForce = 5f;
    public float GroundCheckDist = 1.5f;
    public float LookSensitivityX = 1f;
    public float LookSensitivityY = 1f;
    public float MaxYLookAngle = 90f;
    public float MinYLookAngle = -90f;
    public float Gravity = -9.8f;
    public bool touchingTerrain;
    int targetLayer = 1;

    private float verticalRotation = 0f;

    private Vector3 velocity;
    public Transform PlayerCamera;
    public CharacterController characterController;
    public Ground_Detector detector;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Movement
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        float speed = WalkSpeed;
        if (Input.GetAxis("Sprint") > 0)
        {
            speed *= SprintMultiplier;
        }

        characterController.Move(moveDirection * speed * Time.deltaTime);
        
        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = JumpForce;
        }
        else if (!IsGrounded()) // && !TouchingTerrain()
        {
            velocity.y += Gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);

        //Camera
        if (PlayerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, MinYLookAngle, MaxYLookAngle);

            PlayerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector2.up * mouseX);
        }

        //Debug.Log(IsGrounded());
        //Debug.Log(TouchingTerrain());

    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDist, targetLayer, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        return false;
    }

    //bool TouchingTerrain()
    //{
    //    if (detector.IsHittingTerrain())
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

}
