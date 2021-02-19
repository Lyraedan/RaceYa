using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit button was pressed");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Test");
        Debug.Log("the new game button was pressed");
    }
}