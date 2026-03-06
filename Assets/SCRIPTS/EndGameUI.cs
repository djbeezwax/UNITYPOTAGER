using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private string sceneToReload = "Level1_Potager";
    [SerializeField] private string menuScene = "Menu";

    private void Awake()
    {
        if (endPanel != null) endPanel.SetActive(false);
    }

    public void ShowWin()
    {
        if (endPanel != null) endPanel.SetActive(true);
        Time.timeScale = 0f; // on freeze le jeu
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToReload);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}