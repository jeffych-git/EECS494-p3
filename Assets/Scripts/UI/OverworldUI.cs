using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldUI : MonoBehaviour
{
    private void Start()
    {
        EventBus.Subscribe<CloseInventory>(OnCloseInventory);
        EventBus.Subscribe<OpenInventory>(OnOpenInventory);
    }
    private void Update()
    {
        if(Input.GetKeyDown("e"))
        {
            Inventory();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory();
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void Inventory()
    {
        if (!IsSceneLoaded("InventoryMenu"))
        {
            EventBus.Publish(new OpenInventory());
        }
    }


    private void OnCloseInventory(CloseInventory e)
    {
        SceneManager.UnloadSceneAsync("InventoryMenu");
        Time.timeScale = 1;
    }
    private void OnOpenInventory(OpenInventory e)
    {
        Time.timeScale = 0;
        // Load the additive scene
        SceneManager.LoadSceneAsync("InventoryMenu", LoadSceneMode.Additive);
    }

    private bool IsSceneLoaded(string sceneName)
    {
        // Get the scene by name
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // Check if the scene is loaded
        return scene.isLoaded;
    }
}
