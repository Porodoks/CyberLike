using System;
using System.Collections.Generic;

public abstract class FSMState : IDisposable 
{
    public FSMState(FSMContext context) => this.context = context;
    protected readonly FSMContext context;

    protected List<Func<Type>> transitions = new List<Func<Type>>(8);
    protected List<Func<Type>> personalTransitions = new List<Func<Type>>(8);

    public event Action<Type> StateChangeRequest;

    public virtual void AddTransition(Func<Type> transition)
    {
        if (!transitions.Contains(transition))
            transitions.Add(transition);

    }
    protected virtual void AddPersonalTransition(Func<Type> transition)
    {
        if (!personalTransitions.Contains(transition))
            personalTransitions.Add(transition);
    }
    public abstract void EnterState();
    public abstract void Update();
    public virtual void CheckTransitions()
    {
        foreach (var transition in transitions)
        {
            Type type = transition.Invoke();

            if (type != null)
            {
                StateChangeRequest?.Invoke(type);
                return;
            }
        }
    }

    protected virtual void CheckPersonalTransitions()
    {
        foreach (var transition in personalTransitions)
        {
            Type type = transition.Invoke();

            if (type != null)
            {
                StateChangeRequest?.Invoke(type);
                return;
            }
        }
    }
    public abstract void ExitState();
    public virtual void Dispose()
    {
        return;
    }
    protected void StateChangeRequestInvoke(Type type)
    {
        StateChangeRequest?.Invoke(type);
    }
}
