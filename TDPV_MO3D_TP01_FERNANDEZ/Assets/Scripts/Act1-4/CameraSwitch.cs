using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;

    public Button switchCameraButton;
    public Button movePositionButton;

    private Transform[] cameraMarkers;
    private int currentMarkerIndex = 0;

    void Start()
    {
        // Buscar todos los marcadores en la escena
        GameObject[] markers = GameObject.FindGameObjectsWithTag("CameraMarker");
        cameraMarkers = new Transform[markers.Length];
        for (int i = 0; i < markers.Length; i++)
        {
            cameraMarkers[i] = markers[i].transform;
        }

        // Configurar botones
        switchCameraButton.onClick.AddListener(SwitchCamera);
        movePositionButton.onClick.AddListener(MoveCameraToNextMarker);

        // Configurar posición inicial de la cámara secundaria
        if (secondaryCamera != null && cameraMarkers.Length > 0)
        {
            secondaryCamera.transform.position = cameraMarkers[0].position;
            secondaryCamera.transform.rotation = cameraMarkers[0].rotation;
        }

        UpdateCameraState();
    }

    void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        secondaryCamera.enabled = !secondaryCamera.enabled;
    }

    void MoveCameraToNextMarker()
    {
        if (cameraMarkers.Length == 0 || secondaryCamera == null) return;

        // Incrementar índice
        currentMarkerIndex = (currentMarkerIndex + 1) % cameraMarkers.Length;

        // Mover cámara secundaria
        secondaryCamera.transform.position = cameraMarkers[currentMarkerIndex].position;
        secondaryCamera.transform.rotation = cameraMarkers[currentMarkerIndex].rotation;
    }

    void UpdateCameraState()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }
}
