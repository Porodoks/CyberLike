using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D SelfCollider;
    [HideInInspector]
    public Transform SelfTransform;

    public const float SkinWidth = 0.015f;
    protected const float raySpacing = 0.25f;
    protected int horizontalRayCount;
    protected int verticalRayCount;
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;
    protected RaycastOrigins raycastOrigins;
    public CollisionInfo Collisions;

    protected virtual void Awake()
    {
        SelfCollider = GetComponent<BoxCollider2D>();
        SelfTransform = GetComponent<Transform>();
        CalculateRaySpacings();
    }

    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = SelfCollider.bounds;
        bounds.Expand(SkinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacings()
    {
        Bounds bounds = SelfCollider.bounds;
        bounds.Expand(SkinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / raySpacing);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / raySpacing);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    protected struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public int faceDirection;
        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
