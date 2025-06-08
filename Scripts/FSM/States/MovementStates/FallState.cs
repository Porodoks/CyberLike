using UnityEngine;

public class FallState : MovementState
{
    public FallState(FSMContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        context.Animator.SetTrigger("Fall");
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        velocity = movementController.Velocity;
        velocity.x = GetXSmoothing(movementController.MovementData.RunSpeed * movementController.Input.x);
        AddGravity();
        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
}
