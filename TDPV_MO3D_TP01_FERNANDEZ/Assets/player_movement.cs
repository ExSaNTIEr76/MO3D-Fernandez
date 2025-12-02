using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float groundAccel = 50f;
    public float airAccel = 25f;
    public float groundDecel = 40f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer;

    [Header("Ground Memory (Fix Landing Freeze)")]
    public float groundMemoryTime = 0.08f; 
    private float groundMemoryTimer;

    [Header("Gravity")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody rb;
    private float input;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        // RAW GROUND CHECK:
        bool groundedNow = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (groundedNow)
            groundMemoryTimer = groundMemoryTime;
        else
            groundMemoryTimer -= Time.deltaTime;

        isGrounded = groundMemoryTimer > 0f;

        // COYOTE TIME:
        if (groundedNow)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // SALTO:
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            coyoteTimer = 0f;
        }

        // GRAVEDAD:
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float targetSpeed = input * moveSpeed;

        bool landingHard = rb.velocity.y < -20f;

        float accel = isGrounded && !landingHard
            ? (input != 0 ? groundAccel : groundDecel)
            : airAccel;

        float newSpeed = Mathf.MoveTowards(rb.velocity.x, targetSpeed, accel * Time.fixedDeltaTime);

        rb.velocity = new Vector3(newSpeed, rb.velocity.y, 0);
    }
}
