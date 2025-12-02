using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Trampoline Settings")]
    public float bounceForce = 20f;
    public LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) == 0)
            return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;

        if (collision.relativeVelocity.y <= 0)
        {
            Vector3 vel = rb.velocity;
            vel.y = bounceForce;
            rb.velocity = vel;
        }
    }
}
