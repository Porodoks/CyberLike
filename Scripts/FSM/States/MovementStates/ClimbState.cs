using System;
using System.Collections;
using UnityEngine;

public class ClimbState : MovementState
{
    public ClimbState(FSMContext context, LedderClimbing ledderFinder) : base(context)
    {
        this.ledderClimbing = ledderFinder;
        ledderFinder.OnClimbEnd += OnClimbEnd;
        contextTransform = context.transform;
    }
    private LedderClimbing ledderClimbing;
    private Transform contextTransform;

    public event Action OnCLimbEnd;
    public override void EnterState()
    {
        ledderClimbing.FindLedderPoint(movementController.Input.y == -1);
        Vector3 correction = ledderClimbing.StartClimbing();
        if (correction == Vector3.zero)
        {
            context.ForceStateChange();
            return;
        }

        movementController.ChangeVelocity(Vector2.zero);
        context.transform.position = correction;
        context.Animator.SetTrigger("StartClimb");
    }

    public override void ExitState()
    {
    }

    private void OnClimbEnd(int direction)
    {
        if (direction == 1)
            contextTransform.position += new Vector3(0, 2.2f);
        else
            contextTransform.position += new Vector3(0, .2f);

            OnCLimbEnd?.Invoke();
        context.ForceStateChange();
    }

    public override void Update()
    {
        Vector2 input = movementController.Input;
        Vector3 position = Vector3.zero;
        if (movementController.InputController.SpacePressed())
        {
            ledderClimbing.EndClimb();
            OnCLimbEnd?.Invoke();
            context.ForceStateChange();
            context.LockTransitions(.3f);
            return;
        }
        if (input.y == 1)
        {
            position = ledderClimbing.Next();
        } 
        else if (input.y == -1)
        {
            position = ledderClimbing.Previous();
        }

        if (position == Vector3.zero)
        {
            return;
        }
        contextTransform.position = position;
    }

}
