using UnityEngine;

public class JumpState : MovementState
{
    public JumpState(FSMContext context) : base(context)
    {
        if (context is IBaseAnimationEvents animEvents)
        {
            _animationEvents = animEvents;
            _animationEvents.OnAnimationEnd += OnAnimationEnd;
        }
        else
        {
            Debug.LogWarning($"{this}: Контекст не реализует интерфейс {nameof(IBaseAnimationEvents)}");
        }
    }
    private IBaseAnimationEvents _animationEvents;
    private bool _jumpEnded;

    public override void EnterState()
    {
        context.Animator.SetTrigger("Jump");
        movementController.ChangeVelocity(new Vector2(movementController.Velocity.x, movementController.MovementData.JumpVelocity));
        _jumpEnded = false;
    }

    public override void ExitState()
    {
    }
    public override void CheckTransitions()
    {
        if (_jumpEnded)
            base.CheckTransitions();
    }
    public override void Update()
    {
        velocity = movementController.Velocity;
        velocity.x = GetXSmoothing(movementController.MovementData.RunSpeed * movementController.Input.x);

        if (movementController.CollisionsController.Collisions.above)
            velocity.y = 0;

        AddGravity();

        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }

    private void OnAnimationEnd()
    {
        if (context.CurrentState.GetType() != GetType())
            return;

        _jumpEnded = true;
    }
}
