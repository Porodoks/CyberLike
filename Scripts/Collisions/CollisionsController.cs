using UnityEngine;

public class CollisionsController : RaycastController
{
    public LayerMask collisionMask;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        Collisions.faceDirection = 1;
    }

    public void Move(Vector2 deltaMove)
    {
        UpdateRaycastOrigins();
        Collisions.Reset();

        VerticalCollision(ref deltaMove);

        if (deltaMove.x != 0)
        {
            Collisions.faceDirection = (int)Mathf.Sign(deltaMove.x);
        }

        HorizontalCollision(ref deltaMove);

        SelfTransform.Translate(deltaMove);
    }

    private void HorizontalCollision(ref Vector2 deltaMove)
    {
        float directionX = Collisions.faceDirection;
        float rayLength = Mathf.Abs(deltaMove.x) + SkinWidth + 0.1f;
        if (Mathf.Abs(deltaMove.x) == 0)
        {
            rayLength = 2 * SkinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = directionX == 1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                deltaMove.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;
                Collisions.left = directionX == -1;
                Collisions.right = directionX == 1;
            }
        }
    }

    private void VerticalCollision(ref Vector2 deltaMove)
    {
        float directionY = -1;
        if (deltaMove.y != 0)
            directionY = Mathf.Sign(deltaMove.y);

        if (directionY == 1)
            return;

        float rayLength = Mathf.Abs(deltaMove.y) + SkinWidth;
        //Debug.Log($"Frame: {Time.frameCount}, deltaMove.y: {deltaMove.y}, directionY: {directionY}");
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = directionY == 1 ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);


            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                //Debug.Log($"  Ray {i}: Hit detected, distance: {hit.distance}");
                deltaMove.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;
                Collisions.below = directionY == -1;
                Collisions.above = directionY == 1;
                //Debug.Log($"Collision detected, deltaMove.y adjusted to: {deltaMove.y}, below: {Collisions.below}, above: {Collisions.above}");
            }
            else
            {
                //Debug.Log("No vertical collision detected.");
            }
        }
    }
}
