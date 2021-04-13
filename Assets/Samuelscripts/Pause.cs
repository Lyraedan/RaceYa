using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool paused;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;

    public bool Paused { get => paused; set => paused = value; }

    void Update()
    {
        Debug.Log("pause script is running");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc was pressed!");
            SetPaused();
        } 
    }
    public void SetPaused()
    {
        paused = !paused;
        if (paused)
        {
            //Time.timeScale = 0;
            PauseMenu.SetActive(true);
            Debug.Log("paused");
        }

        else
        {
            //Time.timeScale = 1;
            PauseMenu.SetActive(false);
            Debug.Log("unpaused");
        }
    }
    public void OptionsButton()
    {
        OptionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("ReworkedMainMenu");
        Debug.Log("the main menu button was pressed");
    }
}
