using UnityEngine;

public class StateTestB : StateBase
{
    public StateTestB(StateMachineBase stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Enter State B");
    }

    public override void UpdateState()
    {
        // This input check requires to enable the old input system in the player settings
        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.SetState(GetStateMachine<StateMachineTest>().StateA);
    }

    public override void ExitState()
    {
        Debug.Log("Exit State B");
    }
}
