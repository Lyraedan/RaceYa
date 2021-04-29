using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableGlobalOptions : MonoBehaviour
{
    public static EnableGlobalOptions instance;

    public GameObject canvas;
    private GameObject foundMenu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void OpenOptions()
    {
        foundMenu = GetMenu();
        foundMenu.SetActive(false);
        canvas.SetActive(true);
    }

    public void CloseOptions()
    {
        foundMenu.SetActive(true);
        canvas.SetActive(false);
        foundMenu = null;
    }

    GameObject GetMenu()
    {
        var scene = SceneManager.GetActiveScene().name.Equals("Test");
        if (!scene)
            return GameObject.FindGameObjectWithTag("MainMenu");
        else
            return GameObject.FindGameObjectWithTag("PauseMenu");
    }
}
