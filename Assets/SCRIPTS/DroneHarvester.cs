using UnityEngine;
using UnityEngine.InputSystem;

public class DroneHarvester : MonoBehaviour
{
    public InputActionReference harvestAction; // E

    private VegPickup currentVeg;

    private void OnEnable()
    {
        harvestAction.action.Enable();
        harvestAction.action.performed += OnHarvest;
    }

    private void OnDisable()
    {
        harvestAction.action.performed -= OnHarvest;
        harvestAction.action.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        var veg = other.GetComponentInParent<VegPickup>();
        if (veg != null) currentVeg = veg;
    }

    private void OnTriggerExit(Collider other)
    {
        var veg = other.GetComponentInParent<VegPickup>();
        if (veg != null && veg == currentVeg) currentVeg = null;
    }

    private void OnHarvest(InputAction.CallbackContext ctx)
    {
        if (currentVeg == null) return;

        currentVeg.Collect();
        currentVeg = null;
    }
}
