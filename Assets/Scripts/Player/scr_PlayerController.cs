using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerController : Singleton<scr_PlayerController>
{
    //Movement
    [SerializeField] private float speed;
    [SerializeField] private float sprintModifier;
    private int direction = 0; //0 = Not walking \ 1-4 = Directions clockwise starting from left
    
    //Camera
    [SerializeField] private GameObject _cam;
    [SerializeField] private float rotationSpeed;

    //Music
    public bool isPlayingMusic = false;
    [SerializeField] private AudioClip[] notes = new AudioClip[8];
    public int[] noteCode = new int[5];
    private int[] lastNotes = { 0, 0, 0, 0, 0 };
    
    
    private CharacterController _controller;
    private AudioSource _audioSource;
    [SerializeField] private Animator _animator;

    private InputManager inputManagerInstance => InputManager.Instance;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isPlayingMusic)
        {
            CheckRotation();
            CheckMove();
        }
        else
        {
            PlayMusic();
        }

        HandleAnimator();
    }
    
    private void CheckRotation()
    {
        //float angle = Mathf.Atan2(inputManagerInstance.LookInput.x,inputManagerInstance.LookInput.y) 
        //    * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
        //var targetRotation = Quaternion.Euler(0, angle, 0);

        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        Vector3 targetPosition = new Vector3(_cam.transform.position.x, transform.position.y, _cam.transform.position.z);
        transform.LookAt(targetPosition);
        
    }
    
    private void CheckMove()
    {
        Vector3 input = new Vector3(inputManagerInstance.MoveInput.x, 0, inputManagerInstance.MoveInput.y);
        if (input.sqrMagnitude >= 0.01f)
        {
            var faceDirection = (input.z * _cam.transform.forward + input.x * _cam.transform.right).normalized;
            Vector3 moveDirection = new Vector3(faceDirection.x, -2f, faceDirection.z);
            _controller.Move(moveDirection * (speed * Time.deltaTime));
            Debug.Log(input);
            if (input.z >= 0.9) {direction = 2;}
            else if (input.z <= -0.9) {direction = 4;}
            else if (input.x >= 0.9) {direction = 1;}
            else if (input.x <= -0.9) {direction = 3;}
        }
        else
        { direction = 0; }

    }

    private void PlayMusic()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "MusicBox_Play")
        {
            var tNote = -1;
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 0;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 1;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 2;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 3;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 4;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 5;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 6;}
            if (Keyboard.current.digit1Key.wasPressedThisFrame) {tNote = 7;}

            if (tNote != -1)
            {
                for (int i = 0; i < 4; i++)
                {
                    lastNotes[i] = lastNotes[i + 1];
                }

                lastNotes[4] = tNote;

                if (lastNotes == noteCode)
                {
                    scr_GameManager.Instance.NextStage();
                }
            }
        }
    }

    public void HandleAnimator()
    {
        if (isPlayingMusic)
        {
            direction = 0;
            _animator.SetBool("IsPlayingMusic", true);
            _animator.SetTrigger("PlayMusic");
        }
        else
        {
            _animator.SetBool("IsPlayingMusic", false);
            _animator.SetInteger("MoveDirection", direction);
        }
    }
}
