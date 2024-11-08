using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject twoTeamController;
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        twoTeamController.GetComponent<TwoTeamController>().enabled = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        twoTeamController.GetComponent<TwoTeamController>().enabled = true;
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void SampleScene()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void Overworld()
    {
        SceneManager.LoadScene("Overworld");
        Time.timeScale = 1;
    }

    public void deck()
    {
        SceneManager.LoadScene("InventoryMenu");
        Time.timeScale = 1;
    }

    private void menuKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //if escape key is pressed, pause menu will appear
    //if escape key is pressed again, pause menu will disappear

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuKeyPress();
        }
    }

}
