using UnityEngine;

/// <summary>
/// Smooth side-scrolling camera for Rugy's 2.5D world.
/// Follows the player horizontally and vertically while keeping the camera
/// aligned to a fixed depth plane.
/// </summary>
public class RugySideScrollerCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Depth")]
    [SerializeField] private float cameraZ = -18f;
    [SerializeField] private bool lockTargetZ = true;
    [SerializeField] private float targetPlaneZ = 0f;

    [Header("Follow")]
    [SerializeField] private float horizontalSmoothTime = 0.12f;
    [SerializeField] private float verticalSmoothTime = 0.18f;
    [SerializeField] private bool followVertical = true;
    [SerializeField] private float lookAheadDistance = 2.25f;
    [SerializeField] private float lookAheadSmoothTime = 0.2f;

    [Header("Bounds")]
    [SerializeField] private bool useBounds;
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 140f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 18f;

    private float horizontalVelocity;
    private float verticalVelocity;
    private float lookAhead;
    private float lookAheadVelocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        float desiredLookAhead = Mathf.Sign(GetTargetHorizontalVelocity()) * lookAheadDistance;
        if (Mathf.Abs(GetTargetHorizontalVelocity()) < 0.05f)
            desiredLookAhead = 0f;

        lookAhead = Mathf.SmoothDamp(
            lookAhead,
            desiredLookAhead,
            ref lookAheadVelocity,
            lookAheadSmoothTime);

        Vector3 desiredPosition = target.position + targetOffset;
        desiredPosition.x += lookAhead;
        desiredPosition.z = cameraZ;

        float newX = Mathf.SmoothDamp(
            transform.position.x,
            desiredPosition.x,
            ref horizontalVelocity,
            horizontalSmoothTime);

        float newY = transform.position.y;
        if (followVertical)
        {
            newY = Mathf.SmoothDamp(
                transform.position.y,
                desiredPosition.y,
                ref verticalVelocity,
                verticalSmoothTime);
        }

        if (useBounds)
        {
            newX = Mathf.Clamp(newX, minX, maxX);
            newY = Mathf.Clamp(newY, minY, maxY);
        }

        transform.position = new Vector3(newX, newY, cameraZ);

        if (lockTargetZ)
        {
            Vector3 targetPosition = target.position;
            targetPosition.z = targetPlaneZ;
            target.position = targetPosition;
        }
    }

    private float GetTargetHorizontalVelocity()
    {
        CharacterController controller = target.GetComponent<CharacterController>();
        if (controller != null)
            return controller.velocity.x;

        Rigidbody body = target.GetComponent<Rigidbody>();
        if (body != null)
            return body.linearVelocity.x;

        return 0f;
    }
}
