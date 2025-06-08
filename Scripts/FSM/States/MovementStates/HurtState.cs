using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HurtState : MovementState
{
    public HurtState(FSMContext context) : base(context)
    {
        
    }

    public override void EnterState()
    {
        context.Animator.SetTrigger("Heart");
        movementController.ChangeVelocity(new Vector2(-movementController.Direction * 3f, 3f));
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        velocity = movementController.Velocity;
        velocity.x = GetXSmoothing();
        AddGravity();
        CheckGravityReset();
        movementController.ChangeVelocity(velocity);
        movementController.CollisionsController.Move(velocity * Time.fixedDeltaTime);
    }
}
