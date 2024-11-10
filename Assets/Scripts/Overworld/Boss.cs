using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public string name;
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
            if (DataManager.Instance.encountered_enemy == name)
            {
                if (BattleDataManager.Instance.is_victory)
                {
                    //print("boss killed");
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
