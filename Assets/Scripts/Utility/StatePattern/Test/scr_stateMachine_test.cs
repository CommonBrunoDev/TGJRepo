using UnityEngine;

public class StateMachineTest : StateMachineBase
{
    // Keep track of states
    private StateTestA _stateA;
    private StateTestB _stateB;

    // Properties of tracking states
    public StateTestA StateA => _stateA;
    public StateTestB StateB => _stateB;

    // Implement the initial state
    protected override StateBase InitialState => _stateA;

    // Initialize all states
    private void Awake() 
    {
        _stateA = new StateTestA(this);
        _stateB = new StateTestB(this);
    }
}
