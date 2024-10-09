using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float moveSpeed = 5f; // Speed of camera movement
	public float lookSpeed = 2f; // Speed of mouse look
	private float tempSens, tempMove;
	public bool frozen;
	public Transform player; // Reference to the player or a focal point for the camera

	private float rotationX = 0f; // Rotation around X-axis (up and down)
	private float rotationY = 0f; // Rotation around Y-axis (left and right)

	private void Start()
	{
		tempSens = lookSpeed;
		tempMove = moveSpeed;
		frozen = false;
	}

	private void Update()
	{
		// Handle camera rotation with mouse
		rotationX -= Input.GetAxis("Mouse Y") * lookSpeed; // Look up/down
		rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Clamp to prevent flipping
		rotationY += Input.GetAxis("Mouse X") * lookSpeed; // Look left/right

		// Apply rotation to camera
		transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

		// Handle movement using WSAD keys
		MoveCamera();
	}

	private void MoveCamera()
	{
		// Get the direction the camera is facing
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);
		Vector3 up = transform.TransformDirection(Vector3.up);
		Vector3 down = transform.TransformDirection(Vector3.down);

		// Input for movement
		float moveDirectionY = Input.GetKey(KeyCode.W) ? 1 : 0; // Forward

		moveDirectionY += Input.GetKey(KeyCode.S) ? -1 : 0; // Backward

		float moveDirectionX = Input.GetKey(KeyCode.D) ? 1 : 0; // Right

		moveDirectionX += Input.GetKey(KeyCode.A) ? -1 : 0; // Left

		float moveDirectionUp = Input.GetKey(KeyCode.Space) ? 1 : 0;

		float moveDirectionDown = Input.GetKey(KeyCode.LeftShift) ? -1 : 0;

		// Calculate the movement vector
		Vector3 moveVector =
			((forward * moveDirectionY) + (right * moveDirectionX) + (down * moveDirectionDown) + (up * moveDirectionUp)).normalized * moveSpeed * Time.deltaTime;

		// Move the camera
		transform.position += moveVector;
	}

	public void ToggleFreezeCamera()
	{
		frozen = !frozen;
		if (frozen)
		{
			moveSpeed = 0;
			lookSpeed = 0;
		}
		else
		{
			moveSpeed = tempMove;
			lookSpeed = tempSens;
		}
	}
}