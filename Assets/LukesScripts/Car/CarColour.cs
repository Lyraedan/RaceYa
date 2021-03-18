using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarColour : MonoBehaviour
{
    public static CarColour instance;

    public Color bodyColour = Color.red;
    public Color characterColour = Color.red;

    public UnityEvent OnStart;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        OnStart?.Invoke();
    }

    public void SaveBody(GameObject preview)
    {
        if (gameObject.activeSelf)
        {
            bodyColour = preview.GetComponent<Image>().material.GetColor("_Color1");
            PlayerPrefs.SetFloat("CAR_BODY_RED", bodyColour.r);
            PlayerPrefs.SetFloat("CAR_BODY_GREEN", bodyColour.g);
            PlayerPrefs.SetFloat("CAR_BODY_BLUE", bodyColour.b);
        }
    }

    public void SaveCharacter(GameObject preview)
    {
        if (gameObject.activeSelf)
        {
            characterColour = preview.GetComponent<Image>().material.GetColor("_Color1");
            PlayerPrefs.SetFloat("CAR_CHARACTER_RED", characterColour.r);
            PlayerPrefs.SetFloat("CAR_CHARACTER_GREEN", characterColour.g);
            PlayerPrefs.SetFloat("CAR_CHARACTER_BLUE", characterColour.b);
        }
    }

    public void LoadBody(GameObject preview)
    {
        float bodyR = PlayerPrefs.GetFloat("CAR_BODY_RED");
        float bodyG = PlayerPrefs.GetFloat("CAR_BODY_GREEN");
        float bodyB = PlayerPrefs.GetFloat("CAR_BODY_BLUE");
        bodyColour = new Color(bodyR, bodyG, bodyB);
        preview.GetComponent<Image>().material.SetColor("_Color1", bodyColour);
    }

    public void LoadCharacter(GameObject preview)
    {
        float characterR = PlayerPrefs.GetFloat("CAR_CHARACTER_RED");
        float characterG = PlayerPrefs.GetFloat("CAR_CHARACTER_GREEN");
        float characterB = PlayerPrefs.GetFloat("CAR_CHARACTER_BLUE");
        characterColour = new Color(characterR, characterG, characterB);
        preview.GetComponent<Image>().material.SetColor("_Color1", characterColour);
    }
}
