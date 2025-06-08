using System;
using UnityEngine;

public abstract class MovementState : FSMState 
{
    protected MovementState(FSMContext context) : base(context)
    {
        if (context is IMovementController movementController)
        {
            this.movementController = movementController;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    protected IMovementController movementController;
    protected float Smoothing;
    protected Vector2 velocity;
    protected virtual float GetXSmoothing(float targetVelocity = 0f)
    {
        Vector2 currentVelocity = movementController.Velocity;
        return Mathf.SmoothDamp(
            currentVelocity.x,
            targetVelocity,
            ref Smoothing,
            movementController.CollisionsController.Collisions.below ?
            movementController.MovementData.AccelerationTimeGrounded :
            movementController.MovementData.AccelerationTimeAirborne
        );
    }

    protected virtual void AddGravity()
    {
        velocity.y += movementController.MovementData.BaseGravity * Time.fixedDeltaTime;
        Debug.Log(movementController.MovementData.BaseGravity);
        velocity.y = Mathf.Clamp(velocity.y, movementController.MovementData.MaxFallSpeed, int.MaxValue);
    }
    protected virtual void CheckGravityReset()
    {
        if (movementController.CollisionsController.Collisions.below
            || movementController.CollisionsController.Collisions.above)
            velocity.y = 0f;
    }
}
