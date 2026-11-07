using System;
using Unity.Mathematics;
using UnityEngine;

public class scr_PlayerController : MonoBehaviour
{
    //Movement
    [SerializeField] private float speed;
    [SerializeField] private float sprintModifier;
    
    //Camera
    [SerializeField] private GameObject _cam;
    [SerializeField] private float rotationSpeed;
    
    //Jump
    private float gravity = 9.81f;
    private float _velocityY = 0f;
    [SerializeField] private float jumpPower;
    [SerializeField] private float fallModifier;
    [SerializeField] private float fastfallModifier;
    [SerializeField] private Transform _groundCheck;

    private CharacterController _controller;

    private InputManager inputManagerInstance => InputManager.Instance;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckRotation();
        CheckMove();
        CheckJumpFall();
    }
    
    private void CheckRotation()
    {
        float angle = Mathf.Atan2(inputManagerInstance.LookInput.x,inputManagerInstance.LookInput.y) 
            * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
        var targetRotation = Quaternion.Euler(0, angle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    private void CheckMove()
    {
        Vector3 input = new Vector3(inputManagerInstance.MoveInput.x, 0, inputManagerInstance.MoveInput.y);
        if (input.sqrMagnitude >= 0.01f)
        {
            var faceDirection = (input.z * _cam.transform.forward + input.x * _cam.transform.right).normalized;
            _controller.Move(faceDirection * (speed * Time.deltaTime));
        }
    }

    public void CheckJumpFall()
    {
        if (_controller.isGrounded ||
            Physics.Raycast(_groundCheck.position, transform.TransformDirection(Vector3.down), 0.09f,
                LayerMask.GetMask("Ground")))
        {
            if (inputManagerInstance.JumpInput)
                _velocityY = jumpPower;
            else
                _velocityY = -1;
        }
        else
        {
            _velocityY -= gravity
                          * fallModifier
                          * Time.deltaTime
                          * (inputManagerInstance.FastfallInput ? fastfallModifier : 1);
        }
        _controller.Move(new Vector3(0,_velocityY,0));
    }
}
