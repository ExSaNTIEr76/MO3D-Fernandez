using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    // Variables para controlar la rotación
    private float rotationSpeed = 100f;
    private Vector3 rotationDirection = Vector3.zero;

    void Update()
    {
        // Rotar el cubo en cada frame según la dirección actual
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
    }

    // Método para empezar la rotación en sentido horario
    public void RotateClockwise()
    {
        rotationDirection = Vector3.up; // Rotación en sentido horario
    }

    // Método para empezar la rotación en sentido anti-horario
    public void RotateCounterClockwise()
    {
        rotationDirection = Vector3.down; // Rotación en sentido anti-horario
    }

    // Método para detener la rotación
    public void StopRotation()
    {
        rotationDirection = Vector3.zero; // Detener la rotación
    }
}

