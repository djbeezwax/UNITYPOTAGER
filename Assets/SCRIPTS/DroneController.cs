using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    [Header("Refs")]
    public Transform tiltRoot; // TILT (visuel uniquement, optionnel)

    [Header("Speeds")]
    public float speed = 6f;
    public float verticalSpeed = 4f;
    public float yawSpeed = 120f;

    [Header("Tilt visuel (optionnel)")]
    public float pitchStep = 2f;
    public float maxPitch = 25f;
    public float tiltSmooth = 12f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference verticalAction;
    public InputActionReference pitchAction; // molette (VISUEL SEULEMENT)

    float pitchAngle;

    void OnEnable()
    {
        moveAction.action.Enable();
        verticalAction.action.Enable();
        pitchAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        verticalAction.action.Disable();
        pitchAction.action.Disable();
    }

    void Update()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();
        float vertical = verticalAction.action.ReadValue<float>();

        // 🔹 Rotation gauche / droite (pivot propre)
        transform.Rotate(Vector3.up, move.x * yawSpeed * Time.deltaTime);

        // 🔹 Avancer / reculer (direction SIMPLE)
        transform.position += transform.forward * (move.y * speed * Time.deltaTime);

        // 🔹 Monter / descendre
        transform.position += Vector3.up * (vertical * verticalSpeed * Time.deltaTime);

        // 🔹 Molette = TILT VISUEL UNIQUEMENT (aucun effet sur la direction)
        float scroll = pitchAction.action.ReadValue<float>();
        if (Mathf.Abs(scroll) > 0.01f)
            pitchAngle = Mathf.Clamp(
                pitchAngle + Mathf.Sign(scroll) * pitchStep,
                -maxPitch,
                maxPitch
            );

        if (tiltRoot != null)
        {
            Quaternion target = Quaternion.Euler(pitchAngle, 0f, 0f);
            tiltRoot.localRotation = Quaternion.Slerp(
                tiltRoot.localRotation,
                target,
                tiltSmooth * Time.deltaTime
            );
        }
    }
}
