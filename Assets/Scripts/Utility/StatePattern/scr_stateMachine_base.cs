using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour
{
    // Current state of the state machine
    private StateBase _currentState;
    public StateBase CurrentState => _currentState;

    // Initial state, must be implemented when inherit this base since it does not know which state is the initial one
    protected abstract StateBase InitialState { get; }

    // This is the logic for how you switching states
    public void SetState(StateBase newState) 
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }

    // Set to the initial state when runtime starts
    protected virtual void Start() 
    {
        SetState(InitialState);
    }

    // Update the current state by Unity's update method
    protected virtual void Update() 
    {
        _currentState?.UpdateState();
    }
}