using UnityEngine;

public class CubeController : MonoBehaviour
{
    private CubeBehaviour cubeBehaviour;

    void Start()
    {
        // Obtener la referencia al componente CubeBehaviour en el mismo objeto
        cubeBehaviour = GetComponent<CubeBehaviour>();
    }

    void Update()
    {
        // Detectar las teclas de entrada del jugador
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cubeBehaviour.RotateClockwise(); // Girar en sentido horario
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cubeBehaviour.RotateCounterClockwise(); // Girar en sentido anti-horario
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            cubeBehaviour.StopRotation(); // Detener la rotación
        }
    }
}
