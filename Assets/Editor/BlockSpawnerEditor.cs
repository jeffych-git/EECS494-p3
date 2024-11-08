using UnityEngine;
using UnityEditor;
using System.Linq; 

[CustomEditor(typeof(BlockSpawner))]
public class BlockSpawnerEditor : Editor
{
    private GameObject parentObject; // Parent GameObject to hold all blocks
    private BlockSpawner spawner;
    private bool isPlacing = false;
    private float gridSize = 1;

    void OnEnable()
    {
        spawner = (BlockSpawner)target;
        parentObject = spawner.parentObject;
        gridSize = spawner.gridSize;
        SceneView.duringSceneGui += OnSceneGUI;
        // Create a parent object if it doesn't exist
        if (parentObject == null)
        {
            Debug.Log("BlockSpawner has no parent object");
        }
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (isPlacing)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            Event currentEvent = Event.current;

            // Set grid size equal to block size to avoid overlap
            //float gridSize = 1.0f; // Adjust based on block prefab size

            // Check if the left mouse button is down or dragging
            if ((currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) && currentEvent.button == 0)
            {
                // Convert mouse position to world position
                Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;

                // Snap the position to the nearest grid point
                Vector3 spawnPosition = new Vector3(
                    Mathf.Round(mousePosition.x / gridSize) * gridSize,
                    Mathf.Round(mousePosition.y / gridSize) * gridSize,
                    0); // Make sure z=0 if you’re working in a 2D plane

                // Check if there's already a BoxCollider at this position
                Collider hitCollider = Physics.OverlapBox(spawnPosition, new Vector3(gridSize / 2, gridSize / 2, gridSize / 2), Quaternion.identity)
                                         .FirstOrDefault(col => col is BoxCollider);
                if (hitCollider == null) // Only place a block if no BoxCollider is at this position
                {
                    GameObject newBlock = Instantiate(spawner.blockPrefab);
                    if (newBlock != null)
                    {
                        newBlock.transform.position = spawnPosition;

                        // Ensure new block has a BoxCollider
                        if (newBlock.GetComponent<BoxCollider>() == null)
                        {
                            newBlock.AddComponent<BoxCollider>();
                        }

                        Undo.RegisterCreatedObjectUndo(newBlock, "Spawn Block");
                    }
                    // Set the parent of the new block
                    newBlock.transform.SetParent(parentObject.transform);
                }

                // Consume the event
                currentEvent.Use();
            }

            // Stop placing blocks when the right mouse button is clicked
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
            {
                isPlacing = false;
                currentEvent.Use();
            }
        }
    }





    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(isPlacing ? "Stop Placing" : "Start Placing"))
        {
            isPlacing = !isPlacing;
        }
    }
}
