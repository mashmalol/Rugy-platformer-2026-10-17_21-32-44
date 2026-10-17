using UnityEngine;

/// <summary>
/// Soft-feeling 2.5D controller for the Rugy prototype.
/// Uses a CharacterController, keeps the player on a configurable Z plane,
/// and includes acceleration, coyote time, jump buffering, and floaty jumps.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Rugy3DPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 28f;
    [SerializeField] private float deceleration = 36f;
    [SerializeField] private float lockedZ = 0f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3.2f;
    [SerializeField] private float gravity = -24f;
    [SerializeField] private float fallGravityMultiplier = 1.35f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float heldJumpFloatSpeed = -2.5f;

    [Header("Visual feel")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float swayAngle = 8f;
    [SerializeField] private float swaySmooth = 12f;

    private CharacterController controller;
    private float horizontalVelocity;
    private float verticalVelocity;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float baseVisualZ;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (visualRoot != null)
            baseVisualZ = visualRoot.localEulerAngles.z;
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        bool jumpPressed = Input.GetButtonDown("Jump");
        bool jumpHeld = Input.GetButton("Jump");

        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (controller.isGrounded)
        {
            coyoteTimer = coyoteTime;
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        float target = input * moveSpeed;
        float rate = Mathf.Abs(target) > 0.01f ? acceleration : deceleration;
        horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, target, rate * Time.deltaTime);

        float appliedGravity = verticalVelocity < 0f ? gravity * fallGravityMultiplier : gravity;
        verticalVelocity += appliedGravity * Time.deltaTime;

        if (!controller.isGrounded && jumpHeld && verticalVelocity < heldJumpFloatSpeed)
            verticalVelocity = heldJumpFloatSpeed;

        Vector3 motion = new Vector3(horizontalVelocity, verticalVelocity, 0f);
        controller.Move(motion * Time.deltaTime);

        Vector3 position = transform.position;
        position.z = lockedZ;
        transform.position = position;

        if (Mathf.Abs(input) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(input) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        UpdateVisualSway(input);
    }

    private void UpdateVisualSway(float input)
    {
        if (visualRoot == null)
            return;

        float targetZ = baseVisualZ - input * swayAngle;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZ);
        visualRoot.localRotation = Quaternion.Slerp(
            visualRoot.localRotation,
            targetRotation,
            swaySmooth * Time.deltaTime);
    }
}
