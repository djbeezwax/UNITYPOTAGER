using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public Transform tiltRoot;

    public float speed = 6f;
    public float verticalSpeed = 4f;
    public float yawSpeed = 120f;

    public float pitchStep = 2f;
    public float maxPitch = 25f;
    public float tiltSmooth = 12f;

    public InputActionReference moveAction;
    public InputActionReference verticalAction;
    public InputActionReference pitchAction;

    Rigidbody rb;
    Vector2 moveInput;
    float verticalInput;
    float pitchAngle;

    float yawAngle; // on contrôle le yaw nous-mêmes

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // IMPORTANT : on empêche toute rotation physique
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // init yaw depuis la rotation actuelle
        yawAngle = transform.eulerAngles.y;
    }

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
        moveInput = moveAction.action.ReadValue<Vector2>();
        verticalInput = verticalAction.action.ReadValue<float>();

        float scroll = pitchAction.action.ReadValue<float>();
        if (Mathf.Abs(scroll) > 0.01f)
        {
            pitchAngle = Mathf.Clamp(pitchAngle + Mathf.Sign(scroll) * pitchStep, -maxPitch, maxPitch);
        }

        if (tiltRoot != null)
        {
            Quaternion target = Quaternion.Euler(pitchAngle, 0f, 0f);
            tiltRoot.localRotation = Quaternion.Slerp(tiltRoot.localRotation, target, tiltSmooth * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // YAW contrôlé (Q/D)
        yawAngle += moveInput.x * yawSpeed * Time.fixedDeltaTime;
        Quaternion yawRot = Quaternion.Euler(0f, yawAngle, 0f);
        rb.MoveRotation(yawRot);

        // Direction basée sur yawRot (pas sur transform.forward)
        Vector3 forward = yawRot * Vector3.forward * (moveInput.y * speed);
        Vector3 up = Vector3.up * (verticalInput * verticalSpeed);

        rb.MovePosition(rb.position + (forward + up) * Time.fixedDeltaTime);
    }
}