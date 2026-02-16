using UnityEngine;
using UnityEngine.InputSystem;

public class CollectorE : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference collectAction;

    [Header("Refs")]
    public ScoreManager scoreManager;

    [Header("Collect")]
    public Transform collectPoint;          // <-- DropPoint ici
    public float collectRadius = 1.5f;
    public LayerMask vegetableLayer;

    private void Awake()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnEnable()
    {
        if (collectAction != null) collectAction.action.Enable();
    }

    private void OnDisable()
    {
        if (collectAction != null) collectAction.action.Disable();
    }

    private void Update()
    {
        if (collectAction == null) return;
        if (collectAction.action.WasPressedThisFrame())
            TryCollect();
    }

    private void TryCollect()
    {
        Vector3 center = (collectPoint != null) ? collectPoint.position : transform.position;

        Collider[] hits = Physics.OverlapSphere(
            center,
            collectRadius,
            vegetableLayer,
            QueryTriggerInteraction.Collide
        );

        if (hits.Length == 0) return;

        foreach (var hit in hits)
        {
            // 1) détruit le légume
            Destroy(hit.gameObject);

            // 2) ajoute du score
            if (scoreManager != null)
                scoreManager.AddScore(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = (collectPoint != null) ? collectPoint.position : transform.position;
        Gizmos.DrawWireSphere(center, collectRadius);
    }
}
