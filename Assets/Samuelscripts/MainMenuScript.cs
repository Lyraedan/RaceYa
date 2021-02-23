using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;

    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit button was pressed");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("PregameLobby");
        Debug.Log("the new game button was pressed");
    }

    public void Options()
    {
        OptionsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }
}