using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour
{
    string prefab_path;
    /*    private string slime_prefab_path = "Prefabs/Enemy_Battle_Prefabs/Slime_Battle";
        private string wolf_prefab_path = "Prefabs/Enemy_Battle_Prefabs/Wolf_Battle";*/
    /*    public Vector3 spawnPosition = new Vector3(0, 0, 0);
        public Quaternion spawnRotation = Quaternion.identity;*/


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
        if (scene.name == "PvE") // Check if it's the correct scene
        {
            prefab_path = "Prefabs/Enemy_Battle_Prefabs/" + DataManager.Instance.encountered_enemy + "_Battle";


            // Load the prefab from the Resources folder
            GameObject prefab = Resources.Load<GameObject>(prefab_path);

            if (prefab != null)
            {
                // Instantiate the prefab at the specified position and rotation
                GameObject newObject = Instantiate(prefab);

                // Set the current GameObject as the parent of the new object
                newObject.transform.SetParent(transform);

                /*            // Optionally, reset local position/rotation to align with the parent
                            newObject.transform.localPosition = spawnPosition;
                            newObject.transform.localRotation = spawnRotation;*/
            }
            else
            {
                Debug.LogWarning("Prefab not found at path: " + prefab_path);
            }
        }
        else if(scene.name == "Tutorial")
        {
            prefab_path = "Prefabs/tutorial/tutorial_Battle";


            // Load the prefab from the Resources folder
            GameObject prefab = Resources.Load<GameObject>(prefab_path);

            if (prefab != null)
            {
                // Instantiate the prefab at the specified position and rotation
                GameObject newObject = Instantiate(prefab);

                // Set the current GameObject as the parent of the new object
                newObject.transform.SetParent(transform);

                /*            // Optionally, reset local position/rotation to align with the parent
                            newObject.transform.localPosition = spawnPosition;
                            newObject.transform.localRotation = spawnRotation;*/
            }
            else
            {
                Debug.LogWarning("Prefab not found at path: " + prefab_path);
            }
        }
    }
}