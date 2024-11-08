using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public void OnSinglePlayerClick()
    {
        // Loads Sample Scene
        SceneManager.LoadScene("Overworld");
    }

    public void OnVersusClick()
    {
        // Immediately loads the versus scene
        SceneManager.LoadScene("PvP");
    }

    public void OnQuitClick()
    {
        // This will quit the applicaton when not running in editor
        // May not work if an IOS or mobile build is used
        Application.Quit();
    }
}
