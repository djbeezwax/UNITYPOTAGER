using UnityEngine;

public class DroneCellInteractor : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask cellLayer;

    [Header("Anti 'planter à côté'")]
    [Tooltip("Distance max entre le point touché et le centre de la case pour accepter l'action.")]
    [SerializeField] private float maxOffsetFromCellCenter = 0.6f;

    [Header("Feedback UI")]
    [SerializeField] private ActionFeedbackUI feedback;

    [Header("SFX (optional)")]
    [SerializeField] private bool playSfx = true;

    [Header("VFX (optional)")]
    [SerializeField] private ParticleSystem plantFX;
    [SerializeField] private ParticleSystem waterFX;
    [SerializeField] private ParticleSystem harvestFX;

    [Header("Debug")]
    [SerializeField] private bool debugRay = true;

    public Cell CurrentCell { get; private set; }
    private Vector3 _lastHitPoint;

    void Awake()
    {
        if (feedback == null) feedback = FindFirstObjectByType<ActionFeedbackUI>();
    }

    void Update()
    {
        UpdateCurrentCell();

        if (Input.GetKeyDown(KeyCode.F)) TryPlant();
        if (Input.GetKeyDown(KeyCode.G)) TryWater();
        if (Input.GetKeyDown(KeyCode.E)) TryHarvest();
    }

    void UpdateCurrentCell()
    {
        CurrentCell = null;

        if (rayOrigin == null) return;

        Ray ray = new Ray(rayOrigin.position, Vector3.down);
        if (debugRay) Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, cellLayer, QueryTriggerInteraction.Collide))
            return;

        _lastHitPoint = hit.point;

        // si le collider touché est un enfant, on récupère la Cell au-dessus
        Cell cell = hit.collider.GetComponentInParent<Cell>();
        if (cell == null) return;

        // anti “planter à côté” : on accepte seulement si on est proche du centre de la case
        Vector3 center = cell.transform.position;
        float dx = Mathf.Abs(hit.point.x - center.x);
        float dz = Mathf.Abs(hit.point.z - center.z);

        if (dx <= maxOffsetFromCellCenter && dz <= maxOffsetFromCellCenter)
            CurrentCell = cell;
        else
            CurrentCell = null;
    }

    void TryPlant()
    {
        if (CurrentCell == null) { feedback?.Show("Pas de case"); SfxFail(); return; }

        bool ok = CurrentCell.TryPlant();
        if (ok)
        {
            feedback?.Show("Planté");
            SfxPlant();
            PlayVfx(plantFX, CurrentCell.transform.position);
        }
        else
        {
            feedback?.Show("Déjà occupé");
            SfxFail();
        }
    }

    void TryWater()
    {
        if (CurrentCell == null) { feedback?.Show("Pas de case"); SfxFail(); return; }

        bool ok = CurrentCell.TryWater();
        if (ok)
        {
            feedback?.Show("Arrosé");
            SfxWater();
            PlayVfx(waterFX, CurrentCell.transform.position);
        }
        else
        {
            feedback?.Show("Rien à arroser");
            SfxFail();
        }
    }

    void TryHarvest()
    {
        if (CurrentCell == null) { feedback?.Show("Pas de case"); SfxFail(); return; }

        bool ok = CurrentCell.TryHarvest();
        if (ok)
        {
            feedback?.Show("Récolté");
            SfxHarvest();
            PlayVfx(harvestFX, CurrentCell.transform.position);
        }
        else
        {
            feedback?.Show("Pas mûr");
            SfxFail();
        }
    }

    void PlayVfx(ParticleSystem ps, Vector3 cellPos)
    {
        if (ps == null) return;

        // petit offset pour être visible au-dessus du sol
        ps.transform.position = cellPos + Vector3.up * 0.1f;

        // important : reset + play
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Play(true);
    }

    void SfxPlant()   { if (!playSfx) return; AudioFX.I?.Play(AudioFX.I.plant); }
    void SfxWater()   { if (!playSfx) return; AudioFX.I?.Play(AudioFX.I.water); }
    void SfxHarvest() { if (!playSfx) return; AudioFX.I?.Play(AudioFX.I.harvest); }
    void SfxFail()    { if (!playSfx) return; AudioFX.I?.Play(AudioFX.I.fail); }
}