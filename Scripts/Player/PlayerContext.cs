using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CollisionsController), typeof(PlayerInput))]
public class PlayerContext : FSMContext, IMovementController, IBaseAnimationEvents
{
    public CollisionsController CollisionsController { get; private set; }
    public PlayerInput InputController { get; private set; }
    public MovementData MovementData => _movementData;
    [SerializeField] private MovementData _movementData;

    public Vector2 Velocity => _velocity;
    private Vector2 _velocity;
    public Vector2 Input => _input;
    private Vector2 _input;
    public float Direction { get; private set; }

    private CombatController _combatController;
    private LedderClimbing _leddderClimbing;

    public event Action OnAnimationStart;
    public event Action OnAnimationEnd;

    protected override void Awake()
    {
        base.Awake();
        CollisionsController = GetComponent<CollisionsController>();
        InputController = GetComponent<PlayerInput>();
        _combatController = GetComponent<CombatController>();
        _leddderClimbing = GetComponent<LedderClimbing>();

        IdleState idleState = new IdleState(this);
        RunState runState = new RunState(this);
        JumpState jumpState = new JumpState(this);
        AttackState attackState = new AttackState(this, _combatController);
        FallState fallState = new FallState(this);
        CrouchState crouchState = new CrouchState(this);
        ClimbState climbState = new ClimbState(this, _leddderClimbing);
        climbState.OnCLimbEnd += OnCLimbEnd;

        idleState.AddTransition(ToFallStateCondition);
        idleState.AddTransition(ToCrouchStateCondition);
        idleState.AddTransition(ToRunStateCondition);
        idleState.AddTransition(ToJumpStateCondition);
        idleState.AddTransition(ToAttackStateCondition);
        idleState.AddTransition(ToClimbStateCondition);

        runState.AddTransition(ToFallStateCondition);
        runState.AddTransition(ToCrouchStateCondition);
        runState.AddTransition(ToIdleStateCondition);
        runState.AddTransition(ToJumpStateCondition);
        runState.AddTransition(ToAttackStateCondition);
        runState.AddTransition(ToClimbStateCondition);

        jumpState.AddTransition(ToFallStateCondition);
        jumpState.AddTransition(ToClimbStateCondition);
        jumpState.AddTransition(ToRunStateCondition);
        jumpState.AddTransition(ToIdleStateCondition);

        attackState.AddTransition(ToFallStateCondition);
        attackState.AddTransition(ToCrouchStateCondition);
        attackState.AddTransition(ToIdleStateCondition);
        attackState.AddTransition(ToRunStateCondition);

        fallState.AddTransition(ToIdleStateCondition);
        fallState.AddTransition(ToRunStateCondition);
        fallState.AddTransition(ToAttackStateCondition);
        fallState.AddTransition(ToClimbStateCondition);

        AddState(idleState);
        AddState(runState);
        AddState(jumpState);
        AddState(attackState);
        AddState(fallState);
        AddState(crouchState);
        AddState(climbState);

        baseState = fallState;
        SetState(fallState);
    }

    private void Update()
    {
        _input = InputController.GetInputRaw();
        Direction = CollisionsController.Collisions.faceDirection;
        RevertCharackter();
    }

    private void FixedUpdate()
    {
        CurrentState.Update();
        CurrentState.CheckTransitions();
    }

    public void ChangeVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }
    public void RevertCharackter()
    {
        if (Mathf.Sign(CollisionsController.SelfTransform.localScale.x) != Direction)
        {
            Vector3 scale = CollisionsController.SelfTransform.localScale;
            scale.x *= -1;
            CollisionsController.SelfTransform.localScale = scale;
        }
    }
    public Type ToIdleStateCondition()
    {
        if (CollisionsController.Collisions.below && Input.x == 0)
            return typeof(IdleState);

        return default;
    }

    public Type ToRunStateCondition()
    {
        if (CollisionsController.Collisions.below && Input.x != 0)
            return typeof(RunState);

        return default;
    }

    public Type ToJumpStateCondition()
    {
        if (CollisionsController.Collisions.below && InputController.SpacePressed())
            return typeof(JumpState);

        return default;
    }

    public Type ToAttackStateCondition()
    {
        if (InputController.LeftMouseButtonPressed())
            return typeof(AttackState);

        return default;
    }

    public Type ToFallStateCondition()
    {
        if (!CollisionsController.Collisions.below && Velocity.y < 0)
            return typeof(FallState);

        return default;
    }

    public Type ToCrouchStateCondition()
    {
        if (InputController.CtrlPressed() && CollisionsController.Collisions.below)
            return typeof(CrouchState);

        return default;
    }

    [SerializeField]
    private float delayBetweenClimbStateInS = 1f;
    private bool climbDelayExpired = true;
    public Type ToClimbStateCondition()
    {
        if (climbDelayExpired && Input.y != 0 && _leddderClimbing.FindLedderPoint(Input.y == -1))
            return typeof(ClimbState);

        return default;
    }

    public void OnCLimbEnd()
    {
        StartCoroutine(ClimbStateDelay());
    }
    private IEnumerator ClimbStateDelay()
    {
        climbDelayExpired = false;
        yield return new WaitForSeconds(delayBetweenClimbStateInS);
        climbDelayExpired = true;
    }
    public void OnAnimationStartHandler()
    {
        OnAnimationStart?.Invoke();
    }

    public void OnAnimationEndHandler()
    {
        OnAnimationEnd?.Invoke();
    }
}
