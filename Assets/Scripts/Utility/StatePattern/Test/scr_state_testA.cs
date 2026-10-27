using UnityEngine;

public class StateTestA : StateBase
{
    public StateTestA(StateMachineBase stateMachine) : base(stateMachine) { }
    
    public override void EnterState() 
    {
        Debug.Log("Enter State A");
    }

    public override void UpdateState() 
    {
        // This input check requires to enable the old input system in the player settings
        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.SetState(GetStateMachine<StateMachineTest>().StateB);
    }

    public override void ExitState() 
    {
        Debug.Log("Exit State A");
    }
}
