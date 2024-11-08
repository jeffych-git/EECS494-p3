using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;

    //private float minX, maxX, minY, maxY;

    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

       /* float mapWidth = 100f;
        float mapHeight = 74f;

        minX = -(mapWidth / 2) + (cameraWidth / 2);
        maxX = (mapWidth / 2) - (cameraWidth / 2);
        minY = -(mapHeight / 2) + (cameraHeight / 2);
        maxY = (mapHeight / 2) - (cameraHeight / 2);*/
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position;
        desiredPosition.z = transform.position.z;

        /*desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);*/

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
