using UnityEngine;

public enum CameraState
{
    REST,
    ROTATE,
    MOVE
}

public class CameraManager : MonoBehaviour
{

    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get {
            return _instance;
        }
    }

    [Header("Core")]
    public GameObject CameraObject;

    [Space]
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 2f;
    public bool restrictBelowFloor = true;

    [Space]
    [Header("Angle")]
    public Transform targetObject; // The object the camera is looking at
    public float minDistance = 5f; // Minimum distance from the object
    public float maxDistance = 20f; // Maximum distance from the object
    public float minAngle = 10f; // Minimum rotation angle when close to the object
    public float maxAngle = 45f; // Maximum rotation angle when far from the object

    public static CameraState state;
    private Vector3 lastHitPoint;


    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        // Move the camera
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveCamera(horizontal, vertical);

        // Zoom the camera
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scrollWheel);

        // Check for right mouse button input
        if (Input.GetMouseButtonDown(1))
        {
            state = CameraState.ROTATE;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            state = CameraState.REST;
            lastHitPoint = Vector3.zero;
        }
       
        // Orbit around the floor using horizontal scroll only when right mouse button is pressed
        if (state == CameraState.ROTATE)
        {
            Orbit();
        }
    }

    private void MoveCamera(float horizontal, float vertical)
    {

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void ZoomCamera(float scrollWheel)
    {

        transform.Translate(scrollWheel * zoomSpeed * CameraObject.transform.forward, Space.Self);

        // Raycast downward to find the position of the floor
        /*RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Check if the hit object is a child of the floor
            if (hit.transform.IsChildOf(targetObject))
            {
                // Calculate the distance between the camera and the hit point
                float distance = Vector3.Distance(transform.position, hit.point);

                // Clamp the distance between minDistance and maxDistance
                distance = Mathf.Clamp(distance, minDistance, maxDistance);

                // Interpolate the rotation angle based on the distance
                float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
                float targetAngle = Mathf.Lerp(minAngle, maxAngle, t);

                // Update the child camera's rotation based on the target angle
                Quaternion targetRotation = Quaternion.Euler(targetAngle, transform.eulerAngles.y, transform.eulerAngles.z);
                CameraObject.transform.rotation = Quaternion.Lerp(CameraObject.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }*/
    }

    private void Orbit()
    {

        // Cast a ray from the camera to the floor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the point on the floor where the ray hits
            if (lastHitPoint == Vector3.zero)
            {
                lastHitPoint = hit.point;
            }

            // Orbit the camera around the hit point
            transform.RotateAround(lastHitPoint, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
        }
    }

    private void LateUpdate()
    {
        // Restrict camera below the floor if enabled
        if (restrictBelowFloor)
        {
            RestrictBelowFloor();
        }
    }

    private void RestrictBelowFloor()
    {
        float yPos = Mathf.Max(0f, transform.position.y);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
