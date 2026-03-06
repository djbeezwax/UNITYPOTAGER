using UnityEngine;

public class GardenCell : MonoBehaviour
{
    public Transform centerPoint; // assigne CenterPoint dans l’inspector
    public bool occupied;          // empêche replant

    public Vector3 CenterPos => centerPoint ? centerPoint.position : transform.position;

    public bool CanPlant => !occupied;
    public void MarkOccupied() => occupied = true;
    public void Clear() => occupied = false;
}