using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string gameSceneName = "Level1_Potager";

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
