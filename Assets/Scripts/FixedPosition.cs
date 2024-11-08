using UnityEngine;

public class FixedPosition : MonoBehaviour
{
    private Vector3 fixedPosition;
    private Quaternion fixedRotation;

    void Start()
    {
        // Store the initial world position and rotation
        fixedPosition = transform.position;
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Reset the position and rotation to the fixed values
        transform.position = fixedPosition;
        transform.rotation = fixedRotation;
    }
}
