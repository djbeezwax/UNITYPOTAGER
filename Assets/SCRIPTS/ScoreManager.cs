using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Win")]
    [SerializeField] private int winScore = 20;
    [SerializeField] private EndGameUI endGameUI; // drag&drop dans l'inspector

    private int score = 0;
    private bool hasWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // sécurité si pas assigné
        if (endGameUI == null) endGameUI = FindFirstObjectByType<EndGameUI>();
    }

    private void Start() => UpdateUI();

    public void AddScore(int amount)
    {
        if (hasWon) return;

        score += amount;
        UpdateUI();

        if (score >= winScore)
        {
            hasWon = true;
            endGameUI?.ShowWin();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    public int CurrentScore => score;
}