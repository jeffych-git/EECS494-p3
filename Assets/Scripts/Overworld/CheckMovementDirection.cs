using UnityEngine;

public class CheckMovementDirection : MonoBehaviour
{
    public PlayerMovement pm;
    private Animator animator;
    private Vector3 lastPosition;  // Stores the object's last position
    public Vector3 movementDirection;  // Stores the current movement direction

    void Start()
    {
        // Initialize last position to the object's initial position
        lastPosition = transform.position;
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculate the movement direction by subtracting the last position from the current position
        movementDirection = transform.position - lastPosition;

        // Update the last position to the current position for the next frame
        lastPosition = transform.position;

        // Check if there's any movement
        if (movementDirection != Vector3.zero)
        {
            // Normalize the movement direction to get a direction vector (ignoring the speed/magnitude)
            Vector3 normalizedDirection = movementDirection.normalized;

            // Print the movement direction
            Debug.Log("Moving in direction: " + normalizedDirection);
        }
        if (movementDirection.normalized.sqrMagnitude != 0)
        {
            animator.SetFloat("Horizontal", movementDirection.normalized.x);
            animator.SetFloat("Vertical", movementDirection.normalized.y);
        }
        animator.SetBool("isMoving", movementDirection.normalized.sqrMagnitude > 0); // Speed determines idle or walk state
        animator.SetBool("isRunning", pm.isRunning);
    }

    // This method returns the current movement direction as a Vector3
    public Vector3 GetMovementDirection()
    {
        return movementDirection.normalized;
    }
}
