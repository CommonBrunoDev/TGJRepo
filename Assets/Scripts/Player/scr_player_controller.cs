using UnityEngine;

/// <summary>
/// This class handles the player controls, with movement and jump logic. 
/// If add player logic that does not relate to controls like player stats, 
/// recommended to not add there, create another MonoBehaviour script instead
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cam; // Camera Transform, use main camera usually
    
    [SerializeField, Min(0)] private float moveSpeed = 4;  // Move
    [SerializeField, Min(0)] private float jumpHeight = 5;  // Jump
    [SerializeField, Min(0)] private float turnSpeed = 10; // Player Rotation

    [SerializeField, Min(0)] private float gravityScale = 1; // Gravity Scale when grounded or jumping
    [SerializeField, Min(0)] private float fallingGravityScale = 1; // Gravity Scale when falling

    public float gravityValue = 9.81f; // Player Gravity, value should be positive if the player should fall

    public bool canMove = true; // Trigger that enables the player movement
    public bool canJump = true; // Trigger that enables the player jump

    private CharacterController _controller;    // This player will be used by character controller
    private Vector3 _verticalVelocity = Vector3.zero;   // Vertical movement velocity that includes jump velocity and the gravity

    private InputManager InputManager => InputManager.Instance; // Get Input Manager reference
    
    // Current player gravity scale
    private float CurrentGravityScale 
        => _verticalVelocity.y >= 0 || _controller.isGrounded ? gravityScale : fallingGravityScale;
    
    private void Awake() 
    {
        // Initialize the character controller from the gameObject
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable() 
    {
        // Hide the mouse cursor for third player in 3D game when the controller is enabled
        Cursor.visible = false;
    }

    private void OnDisable() 
    {
        // Show the mouse cursor when the controller is enabled
        Cursor.visible = true;
    }

    private void Update() 
    {
        ResolveGrounded();

        // Calculate the vertical velocity
        if (_controller.isGrounded && InputManager.JumpInput)   // Player can jump only when grounded and the jump key is pressed
            _verticalVelocity = GetJumpVelocity();

        _verticalVelocity += GetGravity() * Time.deltaTime;

        // Combind all movement velocity to the final, then multiply the delta time as the result
        var finalMovement = (GetMoveVelocity() + _verticalVelocity) * Time.deltaTime;

        // Record the final movement with both horizontal and vertical to the character controller
        _controller.Move(finalMovement);
    }

    // Horizontal Movement Calculation
    private Vector3 GetMoveVelocity() 
    {
        if (!canMove) return Vector3.zero;

        var inputDirection = new Vector3(InputManager.MoveInput.x, 0, InputManager.MoveInput.y);

        if (inputDirection.sqrMagnitude < 0.01f) return Vector3.zero;

        var faceDirection = inputDirection.z * cam.forward + inputDirection.x * cam.right;

        HandleMoveRotation(InputManager.MoveInput);
        return faceDirection * moveSpeed;
    }

    // Calculation of Player Rotation, lerp can make the rotation smoother 
    private void HandleMoveRotation(Vector2 input) 
    {
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        var targetRotation = Quaternion.Euler(0, targetAngle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // Jump Calculation, Vector3.up means that the player will jump up
    private Vector3 GetJumpVelocity()
    {
        if (!canJump) return Vector3.zero;
        
        float jumpMagnitude = Mathf.Sqrt(2 * jumpHeight * gravityValue * CurrentGravityScale);
        return jumpMagnitude * Vector3.up;
    }

    // Gravity Calculation, Vector3.down means that the player will fall
    private Vector3 GetGravity() => gravityValue * CurrentGravityScale * Vector3.down;

    // Stable the vertical velocity when grounded
    private void ResolveGrounded() 
    {
        if (_controller.isGrounded && _verticalVelocity.y < -2)
            _verticalVelocity.y = -2;
    }
}