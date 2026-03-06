using UnityEngine;

public class Collector : MonoBehaviour
{
    [Header("Refs")]
    public ScoreManager scoreManager;

    [Header("Points")]
    public int pointsParLegume = 1;

    void Awake()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Option 1 : si ton légume a le tag "Vegetable"
        if (other.CompareTag("Vegetable"))
        {
            if (scoreManager != null)
                scoreManager.AddScore(pointsParLegume);

            Destroy(other.gameObject);
        }
    }

    public bool ContainsPoint(Vector3 worldPoint)
{
    // On vérifie si le point touché par le raycast est bien dans la "case"
    // Version simple basée sur le collider.
    Collider col = GetComponent<Collider>();
    if (col == null) return false;

    // Option 1 : bounds (simple et suffisant pour un carré bien aligné)
    return col.bounds.Contains(worldPoint);
}
}
