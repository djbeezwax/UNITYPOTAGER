using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // le drone
    public Vector3 offset = new Vector3(0f, 15f, -10f);
    public float smooth = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smooth * Time.deltaTime
        );

        transform.LookAt(target.position);
    }
}
