using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarColour : MonoBehaviour
{
    public static CarColour instance;

    public Color bodyColour = Color.red;
    public Color characterColour = Color.red;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SaveBody(GameObject preview)
    {
        if (gameObject.activeSelf)
            bodyColour = preview.GetComponent<Image>().material.GetColor("_Color1");
    }

    public void SaveCharacter(GameObject preview)
    {
        if (gameObject.activeSelf)
            characterColour = preview.GetComponent<Image>().material.GetColor("_Color1");
    }
}
