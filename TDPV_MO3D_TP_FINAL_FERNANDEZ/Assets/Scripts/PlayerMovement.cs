using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRB : MonoBehaviour
{
    [Header("Suavizado")]
    [SerializeField] private float smoothTime = 0.05f;
    private Vector3 smoothVelocity = Vector3.zero;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private PlayerStats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();

        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(h, 0f, v).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 gravityVelocity = new Vector3(0, rb.velocity.y, 0);
        Vector3 targetVelocity = inputDirection * stats.moveSpeed;

        Vector3 smoothed = Vector3.SmoothDamp(
            new Vector3(rb.velocity.x, 0f, rb.velocity.z),
            targetVelocity,
            ref smoothVelocity,
            smoothTime
        );

        rb.velocity = smoothed + gravityVelocity;
    }
}
