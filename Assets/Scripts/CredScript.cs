using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CredScript : MonoBehaviour
{

    public Button MenuButton;

    void Start()
    {
        MenuButton.onClick.AddListener(MainMenu);   
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

