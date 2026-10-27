using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float moveSpeed = 4;  // Move
    [SerializeField, Min(0)] private float jumpHeight = 5;  // Jump
    [SerializeField, Min(0.1f)] private float turnSpeed = 10; // Player Rotation

    [SerializeField, Min(0)] private float gravityScale = 1; // Gravity Scale when landed or jumping
    [SerializeField, Min(0)] private float fallingGravityScale = 1; // Gravity Scale when falling

    public float gravityValue = 9.81f; // Player Gravity, value should be positive if the player should be falling down

    private CharacterController _controller;
    private Vector3 _verticalMoveVelocity = Vector3.zero;
    private Vector3 _faceDirection;

    private InputManager InputManager => InputManager.Instance;
    private float CurrentGravityScale => _verticalMoveVelocity.y >= -2 ? gravityScale : fallingGravityScale;
    
    private void Awake() 
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() 
    {
        ResolveGrounded();

        if (_controller.isGrounded && InputManager.JumpInput)
            _verticalMoveVelocity = GetJumpVelocity();

        _verticalMoveVelocity += GetGravity() * Time.deltaTime;
        
        var finalMovement = (GetMoveVelocity() + _verticalMoveVelocity) * Time.deltaTime;
        HandleMoveRotation();

        _controller.Move(finalMovement);
    }

    private Vector3 GetMoveVelocity() 
    {
        _faceDirection = new Vector3(InputManager.MoveInput.x, 0, InputManager.MoveInput.y).normalized;
        return _faceDirection * moveSpeed;
    }

    private Vector3 GetJumpVelocity() 
    {
        float jumpMagnitude = Mathf.Sqrt(2 * jumpHeight * gravityValue * CurrentGravityScale);
        return jumpMagnitude * Vector3.up;
    }

    private Vector3 GetGravity() => gravityValue * CurrentGravityScale * Vector3.down;

    private void HandleMoveRotation() 
    {
        if (_faceDirection.sqrMagnitude >= 0.01f) 
        {
            var targetRotation = Quaternion.LookRotation(_faceDirection);

            float angle = Quaternion.Angle(transform.rotation, targetRotation);
            float delta = angle * turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, delta);
        }
    }

    private void ResolveGrounded() 
    {
        if (_controller.isGrounded && _verticalMoveVelocity.y < -2)
            _verticalMoveVelocity.y = -2;
    }
}