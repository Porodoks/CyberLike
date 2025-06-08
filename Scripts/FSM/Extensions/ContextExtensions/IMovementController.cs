using UnityEngine;

public interface IMovementController
{
    public CollisionsController CollisionsController { get; }
    public MovementData MovementData { get; }
    public PlayerInput InputController { get; }
    public Vector2 Velocity { get; }
    public Vector2 Input { get; }
    public float Direction { get; }
    public void ChangeVelocity(Vector2 velocity);
}
