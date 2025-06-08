using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Перемещение")]
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private float _moveSpeed = 6f;

    public float RunSpeed => _runSpeed;
    [SerializeField] private float _runSpeed = 9f;
    public float DashSpeed => _dashSpeed;
    [SerializeField] private float _dashSpeed = 9f;
    public float MaxFallSpeed => _maxFallSpeed;
    [SerializeField] private float _maxFallSpeed = -20f;

    public float AccelerationTimeAirborne => _accelerationTimeAirborne;
    [SerializeField] private float _accelerationTimeAirborne = .2f;

    public float AccelerationTimeGrounded => _accelerationTimeGrounded;
    [SerializeField] private float _accelerationTimeGrounded = .1f;

    public float BaseGravity { get; private set; }


    [Header("Прыжок")]
    public float JumpHeight => _jumpHeight;
    [SerializeField] private float _jumpHeight = 2f;

    public float TimeToJumpApex => _timeToJumpApex;
    [SerializeField] private float _timeToJumpApex = .4f;

    public float JumpVelocity { get; private set; }


    [Header("Прилипание к стене")]
    public float WallClipTime => _wallClipTime;
    [SerializeField] private float _wallClipTime = 1f;


    [Header("Отскоки от стены")]
    public Vector2 WallJumpLeapVector => _wallJumpLeapVector;
    [SerializeField] private Vector2 _wallJumpLeapVector;

    public Vector2 WallJumpOffVector => _wallJumpOffVector;
    [SerializeField] private Vector2 _wallJumpOffVector;

    public Vector2 WallJumpClimbVector => _wallJumpClimbVector;
    [SerializeField] private Vector2 _wallJumpClimbVector;

    private void OnValidate()
    {
        CalculateBaseGravity();
        CalculateJumpVelocity();
    }

    private void OnEnable()
    {
        CalculateBaseGravity();
        CalculateJumpVelocity();
    }
    private void CalculateBaseGravity()
    {
        BaseGravity = -(_jumpHeight * 2) / Mathf.Pow(_timeToJumpApex, 2);
    }

    private void CalculateJumpVelocity()
    {
        JumpVelocity = Mathf.Abs(BaseGravity) * _timeToJumpApex;
    }
}
