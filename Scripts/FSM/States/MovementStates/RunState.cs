using UnityEngine;

public class RunState : MovementState
{
    public RunState(FSMContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        context.Animator.SetTrigger("Run");
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        velocity = movementController.Velocity;
        velocity.x = GetXSmoothing(movementController.MovementData.RunSpeed * movementController.Input.x);
        AddGravity();
        CheckGravityReset();
        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
}
