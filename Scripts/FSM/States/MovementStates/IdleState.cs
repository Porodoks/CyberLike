using UnityEngine;

public class IdleState : MovementState
{
    public IdleState(FSMContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        context.Animator.SetTrigger("Idle");
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        velocity = movementController.Velocity;

        if (Mathf.Abs(velocity.x) > 0.1f)
            velocity.x = GetXSmoothing();
        else
            velocity.x = 0;

        AddGravity();
        CheckGravityReset();

        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
}
