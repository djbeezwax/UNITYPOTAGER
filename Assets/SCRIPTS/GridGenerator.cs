using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Refs")]
    public GameObject cellPrefab;

    [Header("Grid")]
    public int width = 5;
    public int height = 5;
    public float spacing = 1.2f;
    public float yOffset = 0.01f;

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("[GridGenerator] cellPrefab is NULL");
            return;
        }

        // Clear children
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        // Center grid around GridRoot
        float xStart = -((width - 1) * spacing) * 0.5f;
        float zStart = -((height - 1) * spacing) * 0.5f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 localPos = new Vector3(xStart + x * spacing, yOffset, zStart + z * spacing);
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.name = $"Cell_{x}_{z}";
                cell.transform.localPosition = localPos;
               
            }
        }

        Debug.Log($"[GridGenerator] Generated {width * height} cells");
    }
}
