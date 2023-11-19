using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target; // The target object to orbit around
    public float rotationSpeed = 3f; // Rotation speed with ZQSD keys
    public float zoomSpeed = 3f; // Zoom speed with mouse scroll
    public float minZoom = 5f; // Minimum zoom distance
    public float maxZoom = 20f; // Maximum zoom distance
    public bool preventBelowFloor = true; // Toggle to prevent camera from going below the floor
    private float floorLevel = 0f; // Adjust this if your floor is at a different height

    void Update()
    {

        CenterCameraOnTarget();

        // Orbit around the target with ZQSD keys
        OrbitWithKeys();

        // Zoom in/out with the mouse scroll wheel
        ZoomWithMouseScroll();

        // Optionally prevent camera from going below the floor
        if (preventBelowFloor && target != null)
        {
            PreventBelowFloor();
        }
    }

    private void CenterCameraOnTarget()
    {
        if (target != null)
        {
            // Center the camera's X and Z coordinates on the target
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
    }

    private void OrbitWithKeys()
    {
        if (target == null)
        {
            return;
        }

        // Rotate around the target using ZQSD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float rotationAngle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(target.position, Vector3.up, horizontalInput * rotationAngle);
        transform.RotateAround(target.position, transform.right, verticalInput * rotationAngle);
    }

    private void ZoomWithMouseScroll()
    {
        // Zoom in/out with the mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollInput * zoomSpeed * Time.deltaTime;

        // Clamp the zoom distance between minZoom and maxZoom
        float newZoom = Mathf.Clamp(transform.position.y - zoomAmount, minZoom, maxZoom);

        // Apply the new zoom distance
        transform.position = new Vector3(transform.position.x, newZoom, transform.position.z);
    }

    private void PreventBelowFloor()
    {
        // Optionally prevent the camera from going below the floor, considering camera rotation
        Vector3 cameraDirection = transform.forward;

        // Check if the camera is facing downward
        if (Vector3.Dot(cameraDirection, Vector3.up) > 0)
        {
            // Calculate the intersection point of the camera's view direction with the floor
            Vector3 intersectionPoint = transform.position - cameraDirection * (transform.position.y - floorLevel) / cameraDirection.y;

            // Adjust the camera's position if it is below the floor
            if (intersectionPoint.y < floorLevel)
            {
                transform.position += Vector3.up * (floorLevel - intersectionPoint.y);
            }
        }
    }


}
