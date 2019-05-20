using UnityEngine;

[AddComponentMenu("Scripts/Camera/Follow Tracking Camera")]
[DisallowMultipleComponent]
public class FollowTrackingCamera : MonoBehaviour
{
    [Tooltip("Camera distance from target in Z axis")]
    public float distance = 20f;

    private float distanceWanted;

    // Options.
    public bool doRotate;

    public bool doZoom;

    [Tooltip("Camera distance from target in Y axis")]
    public float height = 20f;

    private float heightWanted;

    [Tooltip("Camera maximum height/distance limit")]
    public float max = 60;

    [Tooltip("Camera minimum height/distance limit")]
    public float min = 10f;

    // Rotation.
    public float rotateSpeed = 1f;

    private Quaternion rotationResult;

    [Tooltip("Camera target to look at")] public Transform target;

    private Vector3 targetAdjustedPosition;

    // Result vectors.
    private Vector3 zoomResult;

    public float zoomSpeed = 5f;

    // The movement amount when zooming.
    public float zoomStep = 30f;

    private void Start()
    {
        // Initialise default zoom vals.
        heightWanted   = height;
        distanceWanted = distance;

        // Setup our default camera.  We set the zoom result to be our default position.
        zoomResult = new Vector3(0f, height, -distance);
    }

    private void LateUpdate()
    {
        // Check target.
        if (!target)
        {
            Debug.LogError("This camera has no target, you need to assign a target in the inspector.");
            return;
        }

        if (doZoom)
        {
            // Record our mouse input.  If we zoom add this to our height and distance.
            var mouseInput = Input.GetAxis("Mouse ScrollWheel");
            heightWanted   -= zoomStep * mouseInput;
            distanceWanted -= zoomStep * mouseInput;

            // Make sure they meet our min/max values.
            heightWanted   = Mathf.Clamp(heightWanted, min, max);
            distanceWanted = Mathf.Clamp(distanceWanted, min, max);

            height   = Mathf.Lerp(height, heightWanted, Time.deltaTime * zoomSpeed);
            distance = Mathf.Lerp(distance, distanceWanted, Time.deltaTime * zoomSpeed);

            // Post our result.
            zoomResult = new Vector3(0f, height, -distance);
        }

        if (doRotate)
        {
            // Work out the current and wanted rots.
            var currentRotationAngle = transform.eulerAngles.y;
            var wantedRotationAngle  = target.eulerAngles.y;

            // Smooth the rotation.
            currentRotationAngle =
                Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotateSpeed * Time.deltaTime);

            // Convert the angle into a rotation.
            rotationResult = Quaternion.Euler(0f, currentRotationAngle, 0f);
        }

        // Set the camera position reference.
        targetAdjustedPosition = rotationResult * zoomResult;
        transform.position     = target.position + targetAdjustedPosition;

        // Face the desired position.
        transform.LookAt(target);
    }
}