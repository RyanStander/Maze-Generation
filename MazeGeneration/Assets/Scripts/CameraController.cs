using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private float moveSpeed=5, zoomSpeed=5;
    private void Start()
    {
        //Set the main camera
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        //Move up
        if (Input.GetKey(KeyCode.W))
        {
            mainCamera.transform.Translate(Vector3.up*moveSpeed*Time.deltaTime);
        }
        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            mainCamera.transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
        }
        //Move down
        if (Input.GetKey(KeyCode.S))
        {
            mainCamera.transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);
        }
        //Move right
        if (Input.GetKey(KeyCode.D))
        {
            mainCamera.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleZoom()
    {
        //Zoom In
        if (Input.GetKey(KeyCode.Q))
        {
            mainCamera.transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
        }
        //Zoom Out
        if (Input.GetKey(KeyCode.E))
        {
            mainCamera.transform.Translate(-Vector3.forward * zoomSpeed * Time.deltaTime);
        }
    }
}
