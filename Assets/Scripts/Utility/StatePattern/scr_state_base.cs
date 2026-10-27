using UnityEngine;

public abstract class StateBase
{
    protected readonly StateMachineBase stateMachine;

    public StateBase(StateMachineBase stateMachine) 
    {
        this.stateMachine = stateMachine;
    }

    protected T GetStateMachine<T>() where T : StateMachineBase => stateMachine as T;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
