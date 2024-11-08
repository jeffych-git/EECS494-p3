using UnityEngine;

public class SetRenderCameraToMain : MonoBehaviour
{
    void Awake()
    {
        // Get the Canvas component attached to the current GameObject
        Canvas canvas = GetComponent<Canvas>();

        // Check if the Canvas and main camera exist
        if (canvas != null && Camera.main != null)
        {
            // Set the Canvas render mode to Screen Space - Camera (if not already)
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Debug.LogWarning("Set Camera");

            // Set the main camera as the render camera for this Canvas
            canvas.worldCamera = Camera.main;
        }
        else
        {
            Debug.LogWarning("Canvas or Main Camera is missing.");
        }
    }
}
