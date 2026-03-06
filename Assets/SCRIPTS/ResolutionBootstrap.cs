using UnityEngine;

public class ResolutionBootstrap : MonoBehaviour
{
    [SerializeField] int width = 1920;
    [SerializeField] int height = 1080;
    [SerializeField] FullScreenMode mode = FullScreenMode.FullScreenWindow;

    void Awake()
    {
        // Force une résolution cohérente en build
        Screen.SetResolution(width, height, mode);
    }
}
