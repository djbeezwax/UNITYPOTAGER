using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DroneInteractor : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference plantAction;   // ex: E
    public InputActionReference waterAction;   // ex: clic droit / autre

    [Header("Detection")]
    public Transform actionOrigin;             // Empty sous le drone (recommandé)
    public float checkDistance = 2.5f;
    public LayerMask gardenMask;               // Layer des cellules (GardenCell)

    [Header("Prefabs")]
    public GameObject seedPrefab;
    public GameObject waterVfxPrefab;          // optionnel (particle)

    [Header("UI")]
    public TextMeshProUGUI promptText;         // "E : Planter"

    [Header("Anti double trigger")]
    public float cooldown = 0.15f;
    private float nextTime;

    [Header("Debug")]
    public bool debugRay;

    private GardenCell currentCell;

    void OnEnable()
    {
        if (plantAction) plantAction.action.Enable();
        if (waterAction) waterAction.action.Enable();

        if (plantAction) plantAction.action.performed += OnPlant;
        if (waterAction) waterAction.action.performed += OnWater;
    }

    void OnDisable()
    {
        if (plantAction) plantAction.action.performed -= OnPlant;
        if (waterAction) waterAction.action.performed -= OnWater;

        if (plantAction) plantAction.action.Disable();
        if (waterAction) waterAction.action.Disable();
    }

    void Update()
    {
        DetectCell();
        UpdatePrompt();
    }

    void DetectCell()
    {
        currentCell = null;

        Vector3 origin = actionOrigin ? actionOrigin.position : transform.position;

        if (debugRay)
            Debug.DrawRay(origin, Vector3.down * checkDistance, Color.cyan);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, checkDistance, gardenMask))
        {
            currentCell = hit.collider.GetComponentInParent<GardenCell>();
        }
    }

    void UpdatePrompt()
    {
        if (!promptText) return;

        bool show = currentCell != null && currentCell.CanPlant;
        promptText.gameObject.SetActive(show);

        if (show)
            promptText.text = "E : Planter";
    }

    void OnPlant(InputAction.CallbackContext _)
    {
        if (Time.time < nextTime) return;
        nextTime = Time.time + cooldown;

        if (currentCell == null) return;
        if (!currentCell.CanPlant) return;
        if (!seedPrefab) return;

        // Spawn pile au centre de la cellule
        GameObject seedGO = Instantiate(seedPrefab, currentCell.CenterPos, Quaternion.identity);

        // Lier la graine à la cellule si tu as SeedLink
        //var seedLink = seedGO.GetComponent<SeedLink>();
        //if (seedLink) seedLink.cell = currentCell;

        // Empêche de replanter sur la même cellule
        currentCell.MarkOccupied();
    }

    void OnWater(InputAction.CallbackContext _)
    {
        if (Time.time < nextTime) return;
        nextTime = Time.time + cooldown;

        if (currentCell == null) return;

        // VFX au centre de la cellule sous le drone (donc pas "super loin")
        if (waterVfxPrefab)
            Instantiate(waterVfxPrefab, currentCell.CenterPos, Quaternion.identity);

        // Si une graine est dans la cellule, on appelle Water() sur le script Seed
        Seed seed = FindSeedInCell(currentCell);
        if (seed != null)
        {
            // IMPORTANT : Seed doit avoir une méthode public void Water()
            seed.Water();
        }
    }

    // Cherche une Seed proche du centre de la cellule
    Seed FindSeedInCell(GardenCell cell)
    {
        Collider[] hits = Physics.OverlapSphere(cell.CenterPos, 0.25f);
        foreach (var h in hits)
        {
            var s = h.GetComponentInParent<Seed>();
            if (s) return s;
        }
        return null;
    }
}