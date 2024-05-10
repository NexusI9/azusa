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

    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 2f;
    public bool restrictBelowFloor = true;

    public static CameraState state;

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
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void ZoomCamera(float scrollWheel)
    {
        transform.Translate(Vector3.forward * scrollWheel * zoomSpeed, Space.Self);
    }

    private void Orbit()
    {

        // Cast a ray from the camera to the floor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the point on the floor where the ray hits
            Vector3 hitPoint = hit.point;

            // Orbit the camera around the hit point
            transform.RotateAround(hitPoint, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
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
