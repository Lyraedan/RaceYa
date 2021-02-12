using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool paused;

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
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("paused");
        }

        else
        {
            Time.timeScale = 1;
            transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("unpaused");
        }
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit button was pressed");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("the main menu button was pressed");
    }
}
