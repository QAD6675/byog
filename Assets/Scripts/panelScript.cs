using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class panelScript : MonoBehaviour {
    public Button playButton;
    public Button menuButton;
    public Button credsButton;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        credsButton.onClick.AddListener(OpenCreds);
        menuButton.onClick.AddListener(OpenMenu);
    }
    public void PlayGame()
    {
        Time.timeScale=1;
        gameObject.SetActive(false);
    }
    public void lockCreds(){
        credsButton.enabled=false;
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
    public void OpenCreds()
    {
        SceneManager.LoadScene("creds");
    }
}