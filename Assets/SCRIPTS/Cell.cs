using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellState { Empty, Seeded, Grown }

    [Header("State")]
    public CellState State = CellState.Empty;

    [Header("Prefabs")]
    public GameObject SeedPrefab;
    public GameObject VegPrefab;

    [Header("Growth")]
    public float GrowTime = 5f;

    [Header("Water")]
    [SerializeField] private bool requireWaterToGrow = true;

    private GameObject _currentVisual;
    private float _timer;
    private bool _watered;

    void Update()
    {
        if (State == CellState.Seeded)
        {
            // ✅ Si on exige l'arrosage, la plante ne pousse pas tant qu'elle n'est pas arrosée
            if (requireWaterToGrow && !_watered) return;

            _timer += Time.deltaTime;
            if (_timer >= GrowTime)
                BecomeGrown();
        }
    }

    // ✅ IMPORTANT : méthode demandée par DroneCellInteractor
    public bool ContainsPoint(Vector3 worldPoint)
    {
        var col = GetComponent<Collider>();
        if (col == null) return false;
        return col.bounds.Contains(worldPoint);
    }

    public bool TryPlant()
    {
        if (State != CellState.Empty) return false;

        _timer = 0f;
        _watered = false;
        State = CellState.Seeded;

        if (_currentVisual != null) Destroy(_currentVisual);
        if (SeedPrefab != null)
            _currentVisual = Instantiate(SeedPrefab, transform.position, Quaternion.identity);

        return true;
    }

    public bool TryWater()
    {
        if (State != CellState.Seeded) return false;

        _watered = true;

        // ✅ petit bonus optionnel (mais pas "mûr direct")
        _timer += 0.25f; // très léger
        return true;
    }

    public bool TryHarvest()
    {
        if (State != CellState.Grown) return false;

        if (_currentVisual != null) Destroy(_currentVisual);

        if (VegPrefab != null)
            Instantiate(VegPrefab, transform.position, Quaternion.identity);

        // ✅ Score +1 à la récolte
        ScoreManager.Instance?.AddScore(1);

        State = CellState.Empty;
        _timer = 0f;
        _watered = false;
        return true;
    }

    private void BecomeGrown()
    {
        if (_currentVisual != null) Destroy(_currentVisual);

        State = CellState.Grown;

        if (VegPrefab != null)
            _currentVisual = Instantiate(VegPrefab, transform.position, Quaternion.identity);
    }
}