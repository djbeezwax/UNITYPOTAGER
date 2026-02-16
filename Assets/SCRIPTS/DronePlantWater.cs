using UnityEngine;
using UnityEngine.InputSystem;

public class DronePlantWater : MonoBehaviour
{
    [Header("Refs")]
    public Transform dropPoint;

    [Header("Prefabs")]
    public GameObject seedPrefab;
    public GameObject vegPrefab;

    [Header("Input Actions")]
    public InputActionReference plantAction; // F
    public InputActionReference waterAction; // G

    [Header("Ground detection")]
    [Tooltip("Distance max pour trouver le sol sous le dropPoint")]
    public float groundRayDistance = 50f;

    [Tooltip("Décalage pour éviter de spawn dans le sol")]
    public float spawnYOffset = 0.02f;

    [Tooltip("Layer(s) considérés comme le sol")]
    public LayerMask groundMask = ~0; // tout par défaut

    [Header("Water detection")]
    public float waterRadius = 0.6f;

    private void OnEnable()
    {
        plantAction.action.Enable();
        waterAction.action.Enable();

        plantAction.action.performed += OnPlant;
        waterAction.action.performed += OnWater;
    }

    private void OnDisable()
    {
        plantAction.action.performed -= OnPlant;
        waterAction.action.performed -= OnWater;

        plantAction.action.Disable();
        waterAction.action.Disable();
    }

    void OnPlant(InputAction.CallbackContext ctx)
    {
        if (dropPoint == null || seedPrefab == null) return;

        if (TryGetGroundPoint(out Vector3 groundPos))
        {
            Instantiate(seedPrefab, groundPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Plant: Aucun sol détecté sous le dropPoint.");
        }
    }

    void OnWater(InputAction.CallbackContext ctx)
    {
        if (dropPoint == null || vegPrefab == null) return;

        // On cherche les Seeds proches du point sol (pas la hauteur du drone)
        Vector3 center = dropPoint.position;
        if (TryGetGroundPoint(out Vector3 groundPos))
            center = groundPos;

        Collider[] hits = Physics.OverlapSphere(center, waterRadius);

        foreach (Collider h in hits)
        {
            Seed seed = h.GetComponentInParent<Seed>();
            if (seed != null)
            {
                Vector3 spawnPos = seed.transform.position;
                spawnPos.y = center.y; // colle au sol si seed a une hauteur chelou

                Instantiate(vegPrefab, spawnPos, Quaternion.identity);
                Destroy(seed.gameObject);
                break;
            }
        }
    }

    bool TryGetGroundPoint(out Vector3 groundPos)
    {
        groundPos = dropPoint.position;

        // On lance le raycast depuis un peu au-dessus du dropPoint pour éviter d’être dans un collider
        Vector3 origin = dropPoint.position + Vector3.up * 1.0f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            groundPos = hit.point + Vector3.up * spawnYOffset;
            return true;
        }

        return false;
    }

    // Juste pour debug visuel dans la scène
    private void OnDrawGizmosSelected()
    {
        if (dropPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(dropPoint.position + Vector3.up, dropPoint.position + Vector3.down * groundRayDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(dropPoint.position, waterRadius);
    }
}
