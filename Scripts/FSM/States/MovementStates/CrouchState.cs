using System;
using UnityEngine;

public class CrouchState : MovementState
{
    public CrouchState(FSMContext context) : base(context)
    {
        AddPersonalTransition(ToFallStatePersonalCondition);
        AddPersonalTransition(ToIdleStatePersonalCondition);
        AddPersonalTransition(ToAttackStatePersonalCondition);
        AddPersonalTransition(ToJumpStatePersonalCondition);
    }
    public override void AddTransition(Func<Type> transition)
    {
        Debug.LogWarning($"{this}: Состояние {nameof(CrouchState)} должно определять переходы самостоятельно");
    }
    public override void EnterState()
    {
        context.Animator.SetTrigger("Crouch");
    }
    public override void Update()
    {
        velocity = movementController.Velocity;

        if (Mathf.Abs(velocity.x) > 0.1f)
            velocity.x = GetXSmoothing();
        else
            velocity.x = 0;

        if (Mathf.Abs(movementController.Input.x) > 0 && velocity.x == 0)
            velocity.x = 0.005f * movementController.Input.x;

        AddGravity();
        CheckGravityReset();

        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
    public override void CheckTransitions()
    {
        CheckPersonalTransitions();
    }
    public override void ExitState()
    {

    }

    public Type ToAttackStatePersonalCondition()
    {
        if (movementController.InputController.LeftMouseButtonPressed())
            return typeof(AttackState);

        return default;
    }

    public Type ToJumpStatePersonalCondition()
    {
        if (movementController.InputController.SpacePressed())
            return typeof(JumpState);

        return default;
    }
    public Type ToFallStatePersonalCondition()
    {
        if (!movementController.CollisionsController.Collisions.below && movementController.Velocity.y < 0)
            return typeof(FallState);

        return default;
    }

    public Type ToIdleStatePersonalCondition()
    {
        if (!movementController.InputController.CtrlPressed())
            return typeof(IdleState);

        return default;
    }
}
