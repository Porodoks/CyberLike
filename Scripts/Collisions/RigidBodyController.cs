using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidBodyController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _floorCheckTransform;
    [SerializeField] private float _checkRadius;
    public LayerMask CollisionsMask;
    [HideInInspector]
    public Transform SelfTransform;
    [HideInInspector]
    public CollisionsInfo Collisions;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        SelfTransform = GetComponent<Transform>();
        Collisions.faceDirection = 1;
    }

    public void Move(Vector3 velocity)
    {
        Collisions.Reset();
        _rigidbody.MovePosition(SelfTransform.position + velocity);
        Collider2D floorCheck = Physics2D.OverlapCircle(_floorCheckTransform.position, _checkRadius, CollisionsMask);
        if (floorCheck)
        {
            Collisions.below = true;
        }
    }

    public struct CollisionsInfo
    {
        public bool left, right;
        public bool below, above;

        public float faceDirection;

        public void Reset()
        {
            left = right = false;
            below = above = false;
        }
    }
}
