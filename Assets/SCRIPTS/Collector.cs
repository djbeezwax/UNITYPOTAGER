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
}
