using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float gridSize = 1f; // Size of each grid cell
    public float movementSpeed = 3f; // Speed at which the player moves to the target position
    public float runningMultiplier = 2.5f; // Multiplier for running speed
    public bool isRunning = false;
    public LayerMask obstacleLayer; // Layer to identify obstacles

    //private Animator animator;
    private Vector3 targetPosition; // The next grid point to move to
    private bool isMoving = false; // Flag to prevent multiple moves at once

    void Start()
    {
        // Start by rounding the player’s initial position to the grid
        targetPosition = SnapToGrid(transform.position);
        transform.position = targetPosition;
        //animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Set isRunning based on whether the Shift key is held down
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calculate movement speed based on running state
        float currentSpeed = isRunning ? movementSpeed * runningMultiplier : movementSpeed;

        // If currently moving, skip input checks
        if (isMoving)
        {

        }
        // Check for vertical input first
        else if (Input.GetAxisRaw("Vertical") != 0 && Mathf.Approximately(transform.position.y, targetPosition.y))
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(0, Mathf.Sign(verticalInput) * gridSize, 0);

            // Check for obstacle in the movement direction
            if (!IsObstacleInDirection(direction))
            {
                targetPosition += direction;
                isMoving = true;
            }
        }
        // Check for horizontal input if no vertical input was detected
        else if (Input.GetAxisRaw("Horizontal") != 0 && Mathf.Approximately(transform.position.x, targetPosition.x))
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = new Vector3(Mathf.Sign(horizontalInput) * gridSize, 0, 0);

            // Check for obstacle in the movement direction
            if (!IsObstacleInDirection(direction))
            {
                targetPosition += direction;
                isMoving = true;
            }
        }

        // Move towards the target position based on movementSpeed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime * gridSize);

        // When the target position is reached, allow new input
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition; // Snap exactly to target
            isMoving = false;
        }
    }

    // Method to check if there's an obstacle in the direction of movement
    private bool IsObstacleInDirection(Vector3 direction)
    {
        float distance = gridSize; // Distance to check (one grid unit)
        bool hit = Physics.Raycast(transform.position, direction, distance, obstacleLayer);

        return hit; // Returns true if there's an obstacle
    }


    // Snap to the nearest grid coordinate
    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }


    //To Move To Original Position

    private void OnEnable()
    {
        // Register to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld") // Check if it's the correct scene
        {
            transform.position = DataManager.Instance.player_position;
        }
    }
}
