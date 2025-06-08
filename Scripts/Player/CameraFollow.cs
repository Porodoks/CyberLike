using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public CollisionsController target
    {
        get => _target;
        set
        {
            _target = value;
            CalculateFocusArea();
        }
    }
    private CollisionsController _target;
    public Vector2 focusAreaSize;
    public float verticalOffset;
    public float lookAheadDistance;
    public float lookAheadSmoothTime;
    public float verticalSmoothTIme;

    private FocusArea focusArea;

    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirection;
    private float smoothVelocityX;
    private float smoothVelocityY;

    private bool lookAheadStopped;
    private void Start()
    {
    }

    private void CalculateFocusArea()
    {
        focusArea = new FocusArea(target.SelfCollider.bounds, focusAreaSize);
    }
    private void FixedUpdate()
    {
        focusArea.Update(target.SelfCollider.bounds);
        Vector2 targetPosition = focusArea.center + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirection = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) == Mathf.Sign(focusArea.velocity.x) && Input.GetAxisRaw("Horizontal") != 0)
            {
                targetLookAheadX = lookAheadDirection * lookAheadDistance;
                lookAheadStopped = false;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirection * lookAheadDistance - currentLookAheadX) / 4f;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothVelocityX, lookAheadSmoothTime);

        targetPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref smoothVelocityY, verticalSmoothTIme);
        targetPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)targetPosition + Vector3.forward * -10;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, .5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    private struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        private float left, right;
        private float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;

            bottom = targetBounds.min.y;
            top = targetBounds.max.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);

        }
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
