using UnityEngine;

public class AttackState : MovementState
{
    public AttackState(FSMContext context, CombatController combatController) : base(context)
    {
        _combatController = combatController;
    }
    private CombatController _combatController;

    private float _minStateTimeMS = 200;
    private float _stateChangeDelayTimer = 0f;
    public override void EnterState()
    {
        if (context.PreviousState.GetType() == typeof(RunState))
            context.Animator.SetTrigger("RunAttack");
        else if (context.PreviousState.GetType() == typeof(CrouchState))
            context.Animator.SetTrigger("CrouchAttack");
        else
            context.Animator.SetTrigger("IdleAttack");

        if (_stateChangeDelayTimer <= 0f)
            _combatController.Fire();

        _stateChangeDelayTimer = _minStateTimeMS / 1000;
    }
    public override void Update()
    {
        _stateChangeDelayTimer -= Time.fixedDeltaTime;

        velocity = movementController.Velocity;

        if (context.PreviousState.GetType() == typeof(RunState) || context.PreviousState.GetType() == typeof(FallState))
            velocity.x = GetXSmoothing(movementController.MovementData.RunSpeed * movementController.Input.x);
        else if (Mathf.Abs(velocity.x) > 0.1f)
            velocity.x = GetXSmoothing();
        else
            velocity.x = 0f;

        AddGravity();
        CheckGravityReset();

        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
    public override void CheckTransitions()
    {
        if (_stateChangeDelayTimer <= 0f || (context.PreviousState.GetType() == typeof(FallState) && movementController.CollisionsController.Collisions.below))
            base.CheckTransitions();
    }
    public override void ExitState()
    {

    }

}
