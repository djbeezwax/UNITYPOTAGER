using UnityEngine;

public class VegPickup : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    public void Collect()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreValue);

        Destroy(gameObject);
    }
}
