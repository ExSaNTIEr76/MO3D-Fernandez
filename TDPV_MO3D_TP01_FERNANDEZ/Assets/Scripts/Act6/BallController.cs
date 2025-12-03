using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 1.5f;
    public float minZ = -1f;
    public float maxZ = 1f;

    [Header("Carga y lanzamiento")]
    public float maxChargeTime = 0.8f;
    public float minLaunchForce = 1f;
    public float maxLaunchForce = 45f;
    public Vector3 launchDirection = Vector3.left;

    [Header("Referencias Gauge")]
    public Transform frontGaugeTransform;
    public Transform backGaugeTransform;

    private Rigidbody rb;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool launched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateGaugeVisual(0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
            return;
        }

        if (!launched)
        {
            HandleMovementInput();
            HandleChargeInput();
        }
    }

    // MOVIMIENTO VERTICAL:
    void HandleMovementInput()
    {
        float v = Input.GetAxisRaw("Vertical");
        Vector3 pos = transform.position;
        pos += Vector3.forward * v * moveSpeed * Time.deltaTime;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }


    // CARGA:
    void HandleChargeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeTimer = 0f;
        }

        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > maxChargeTime) chargeTimer = maxChargeTime;

            UpdateGaugeVisual(chargeTimer / maxChargeTime);
        }

        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            float t = Mathf.Clamp01(chargeTimer / maxChargeTime);
            float force = Mathf.Lerp(minLaunchForce, maxLaunchForce, t);

            Launch(force);
            isCharging = false;
            UpdateGaugeVisual(0f);
        }
    }

    void Launch(float force)
    {
        launched = true;
        rb.isKinematic = false;
        rb.AddForce(launchDirection.normalized * force, ForceMode.Impulse);
    }

    // GAUGE:
    void UpdateGaugeVisual(float normalized)
    {
        if (frontGaugeTransform == null) return;
        Vector3 s = frontGaugeTransform.localScale;
        s.y = Mathf.Clamp01(normalized);
        frontGaugeTransform.localScale = s;
    }


    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
