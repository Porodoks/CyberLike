using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class FSMContext : MonoBehaviour
{
    public FSMState CurrentState { get; protected set; }
    public FSMState PreviousState { get; protected set; }
    protected FSMState baseState;

    protected Dictionary<Type, FSMState> states = new Dictionary<Type, FSMState>(8);

    private bool transitionLocked;
    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (CurrentState == null)
        {
            foreach (var item in states)
            {
                SetState(item.Value);
                break;
            }
        }
        if (baseState == null)
        {
            foreach (var item in states)
            {
                baseState = item.Value;
                break;
            }
        }
    }
    protected virtual void OnDestroy()
    {
        foreach (var state in states)
        {
            state.Value.StateChangeRequest -= OnStateChangeRequest;
            state.Value.Dispose();
        }
    }
    public virtual void AddState(FSMState state)
    {
        if (states.ContainsKey(state.GetType()))
        {
            Debug.LogWarning($"{this}: ѕопытка зарегестрировать уже существующее состо€ние");
            return;
        }

        state.StateChangeRequest += OnStateChangeRequest;
        states.Add(state.GetType(), state);
    }
    protected void OnStateChangeRequest(Type type)
    {
        if (!transitionLocked)
            SetState(type);
    }
    public void ForceStateChange()
    {
        SetState(baseState);
    }
    public void SafeForceStateChange()
    {
        StartCoroutine(SafeForceStateChangeCoroutine());
    }
    private IEnumerator SafeForceStateChangeCoroutine()
    {
        yield return new WaitForEndOfFrame();
        SetState(baseState);
    }

    public void LockTransitions(float timeInS)
    {
        StartCoroutine(LockTransitionsCorotine(timeInS));
    }

    private IEnumerator LockTransitionsCorotine(float timeInS)
    {
        transitionLocked = true;
        yield return new WaitForSeconds(timeInS);
        transitionLocked = false;
    }
    protected void SetState(FSMState state)
    {
        if (state == null)
            throw new ArgumentNullException($"{this}: ѕереданное состо€ние равн€етс€ null");

        if (CurrentState != null && CurrentState == state)
            return;

        if (states.TryGetValue(state.GetType(), out var newState))
        {
            PreviousState = CurrentState;
            CurrentState?.ExitState();
            newState.EnterState();
            CurrentState = newState;

            Debug.Log($"{this}: ѕереход из состо€ни€ {PreviousState?.GetType()} в {CurrentState.GetType()}");
        }
        else
        {
            Debug.LogWarning($"{this}: ѕопытка перехода в незарегестрированное состо€ние");
        }
    }

    protected void SetState(Type stateType)
    {
        if (stateType == null)
            throw new ArgumentNullException($"{this}: ѕереданный тип равн€етс€ null");

        if (CurrentState != null && CurrentState.GetType() == stateType)
            return;

        if (states.TryGetValue(stateType, out var newState))
        {
            PreviousState = CurrentState;
            CurrentState?.ExitState();
            newState.EnterState();
            CurrentState = newState;

            Debug.Log($"{this}: ѕереход из состо€ни€ {PreviousState?.GetType()} в {CurrentState.GetType()}");
        }
        else
        {
            Debug.LogWarning($"{this}: ѕопытка перехода в незарегистрированное состо€ние");
        }

    }
    public Animator Animator { get; protected set; }
}
