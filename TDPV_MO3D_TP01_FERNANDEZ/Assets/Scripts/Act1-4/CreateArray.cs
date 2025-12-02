using UnityEngine;

public class CreateArray : MonoBehaviour
{
    // Variables públicas para configurar el prefab y las dimensiones del muro
    public GameObject blockPrefab;
    public int rows = 3;
    public int columns = 4;

    void Start()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("No se ha asignado un prefab para el bloque.");
            return;
        }

        // Obtener todos los renderers en el prefab
        Renderer[] renderers = blockPrefab.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("El prefab no contiene componentes Renderer.");
            return;
        }

        // Calcular los bounds combinados de todos los renderers
        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        // Obtener el tamaño total del bloque usando los bounds combinados
        Vector3 blockSize = combinedBounds.size;
        float xOffset = blockSize.x;
        float yOffset = blockSize.y;

        // Crear el muro en una cuadrícula de NxM
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calcular la posición para cada bloque
                Vector3 position = new Vector3(j * xOffset, i * yOffset, 0);
                Instantiate(blockPrefab, position, Quaternion.identity);
            }
        }
    }
}