using Unity.Cinemachine;
using KBCore.Refs;
using UnityEngine;

public class PlayerController : ValidatedMonoBehaviour {
    [Header("References")]
    [SerializeField, Self] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField, Anywhere] CinemachineCamera freeLookVCam;
    [SerializeField, Anywhere] InputReader input;
    
    [Header("Settings")]
    [SerializeField, Min(0)] float moveSpeed = 6f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float smoothTime = 0.2f;

    const float Zerof = 0f;
    
    Transform mainCam;

    float currentSpeed;
    float velocity;
    

    void Awake() {
        mainCam = Camera.main.transform;
        freeLookVCam.Follow = transform;
        freeLookVCam.LookAt = transform;
        
        // Invoke event when transform is teleported to teleport camera at the same time
        freeLookVCam.OnTargetObjectWarped(
            transform,
            transform.position - freeLookVCam.transform.position - Vector3.forward
            );
    }

    void Update() {
        HandleMovement();
        //UpdateAnimator();
    }

    void HandleMovement() {
        var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
        
        if (adjustedDirection.magnitude > Zerof) {
            HandleRotation(adjustedDirection);
            HandleCharacterController(adjustedDirection);

            SmoothSpeed(adjustedDirection.magnitude);
            
        }
        else {
            SmoothSpeed(Zerof);
        }
    }

    void HandleCharacterController(Vector3 adjustedDirection) {
        var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
        controller.Move(adjustedMovement);
    }

    private void HandleRotation(Vector3 adjustedDirection) {
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.LookAt(transform.position + adjustedDirection);
    }

    void SmoothSpeed(float value) {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }
}