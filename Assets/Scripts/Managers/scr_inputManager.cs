using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerInputs _inputs;

    public Vector2 MoveInput => _inputs.Gameplay.Movement.ReadValue<Vector2>();
    public Vector2 LookInput => _inputs.Gameplay.CameraLook.ReadValue<Vector2>();
    public bool JumpInput => _inputs.Gameplay.Jump.WasPressedThisFrame();

    protected override void Awake() 
    {
        base.Awake();
        _inputs = new PlayerInputs();
    }
    
    private void OnEnable() 
    {
        _inputs?.Enable();
    }

    private void OnDisable() 
    {
        _inputs?.Disable();
    }
}
