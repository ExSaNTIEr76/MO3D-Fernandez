using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraConfigurator : MonoBehaviour
{
    // Referencias a los elementos de la UI
    public Button projectionButton;
    public GameObject perspectiveGroup; // Grupo para FOV slider
    public Slider fovSlider;
    public GameObject orthographicGroup; // Grupo para Size slider
    public Slider sizeSlider;
    public Slider nearClipSlider;
    public Slider farClipSlider;

    private Camera cam;

    void Start()
    {
        // Inicializar la referencia a la cámara
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found!");
            return;
        }

        // Validar que las referencias de UI estén asignadas
        if (projectionButton == null || perspectiveGroup == null || fovSlider == null ||
            orthographicGroup == null || sizeSlider == null || nearClipSlider == null ||
            farClipSlider == null)
        {
            Debug.LogError("Please assign all UI references in the Inspector!");
            return;
        }

        // FOV
        fovSlider.minValue = 10f;
        fovSlider.maxValue = 125f;

       // SIZE
        sizeSlider.minValue = 0.1f;
        sizeSlider.maxValue = 100f;

       // NEAR CLIP PLANE
        nearClipSlider.minValue = 0.01f;
        nearClipSlider.maxValue = 150;

       // FAR CLIP PLANE
        farClipSlider.minValue = 0.01f;
        farClipSlider.maxValue = 150f;

        // Configurar valores iniciales desde la cámara
        fovSlider.value = cam.fieldOfView;
        sizeSlider.value = cam.orthographicSize;
        nearClipSlider.value = cam.nearClipPlane;
        farClipSlider.value = cam.farClipPlane;

        // Asignar listeners a los controles
        projectionButton.onClick.AddListener(ToggleProjection);
        fovSlider.onValueChanged.AddListener(value =>
        {
            cam.fieldOfView = Mathf.Clamp(value, fovSlider.minValue, fovSlider.maxValue);
        });
        sizeSlider.onValueChanged.AddListener(value =>
        {
            cam.orthographicSize = Mathf.Clamp(value, sizeSlider.minValue, sizeSlider.maxValue);
        });
        nearClipSlider.onValueChanged.AddListener(value =>
        {
            // Validar que Near siempre sea menor que Far
            if (value < cam.farClipPlane)
                cam.nearClipPlane = Mathf.Clamp(value, nearClipSlider.minValue, farClipSlider.value - 0.1f);
            else
                nearClipSlider.value = cam.farClipPlane - 0.1f;
        });
        farClipSlider.onValueChanged.AddListener(value =>
        {
            // Validar que Far siempre sea mayor que Near
            if (value > cam.nearClipPlane)
                cam.farClipPlane = Mathf.Clamp(value, nearClipSlider.value + 0.1f, farClipSlider.maxValue);
            else
                farClipSlider.value = cam.nearClipPlane + 0.1f;
        });

        // Actualizar visibilidad inicial de los grupos
        UpdateUI();
    }

    /// <summary>
    /// Alternar entre perspectiva y proyección ortográfica
    /// </summary>
    void ToggleProjection()
    {
        cam.orthographic = !cam.orthographic;
        UpdateUI();
    }

    /// <summary>
    /// Actualizar la visibilidad de los grupos UI según el tipo de proyección
    /// </summary>
    void UpdateUI()
    {
        // Mostrar grupo correspondiente según el modo actual de la cámara
        perspectiveGroup.SetActive(!cam.orthographic);
        orthographicGroup.SetActive(cam.orthographic);
    }

    /// <summary>
    /// Dibujar el Frustum de la cámara como gizmo amarillo
    /// </summary>
    void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.yellow;
        Matrix4x4 tempMatrix = Gizmos.matrix;
        Gizmos.matrix = cam.transform.localToWorldMatrix;

        if (cam.orthographic)
        {
            // Dibujar el frustum ortográfico
            float size = cam.orthographicSize;
            float aspect = cam.aspect;
            float near = cam.nearClipPlane;
            float far = cam.farClipPlane;

            Vector3 nearTopLeft = new Vector3(-size * aspect, size, near);
            Vector3 nearTopRight = new Vector3(size * aspect, size, near);
            Vector3 nearBottomLeft = new Vector3(-size * aspect, -size, near);
            Vector3 nearBottomRight = new Vector3(size * aspect, -size, near);

            Vector3 farTopLeft = nearTopLeft + Vector3.forward * (far - near);
            Vector3 farTopRight = nearTopRight + Vector3.forward * (far - near);
            Vector3 farBottomLeft = nearBottomLeft + Vector3.forward * (far - near);
            Vector3 farBottomRight = nearBottomRight + Vector3.forward * (far - near);

            // Conectar las esquinas
            Gizmos.DrawLine(nearTopLeft, nearTopRight);
            Gizmos.DrawLine(nearTopRight, nearBottomRight);
            Gizmos.DrawLine(nearBottomRight, nearBottomLeft);
            Gizmos.DrawLine(nearBottomLeft, nearTopLeft);

            Gizmos.DrawLine(farTopLeft, farTopRight);
            Gizmos.DrawLine(farTopRight, farBottomRight);
            Gizmos.DrawLine(farBottomRight, farBottomLeft);
            Gizmos.DrawLine(farBottomLeft, farTopLeft);

            Gizmos.DrawLine(nearTopLeft, farTopLeft);
            Gizmos.DrawLine(nearTopRight, farTopRight);
            Gizmos.DrawLine(nearBottomLeft, farBottomLeft);
            Gizmos.DrawLine(nearBottomRight, farBottomRight);
        }
        else
        {
            // Dibujar frustum en modo perspectiva
            Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        }

        Gizmos.matrix = tempMatrix;
    }
}
