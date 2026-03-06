using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Water")]
    public bool isWatered;

    // Appelé par DroneInteractor quand tu arroses la cellule sous le drone
    public void Water()
    {
        isWatered = true;
    }

    // Optionnel: si tu veux pouvoir "consommer" l'arrosage (ex: une fois)
    public void ConsumeWater()
    {
        isWatered = false;
    }
}