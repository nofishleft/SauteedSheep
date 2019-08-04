using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsCanvas;
    public GameObject menuCanvas;
    public AudioSource menuSrc;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(settingsCanvas);
        DontDestroyOnLoad(pauseMenu);
    }

    public bool ShowingSettings = false;
    public bool ShowingPause = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().name != "Menu")
            {
                if (ShowingSettings)
                {
                    PauseMenuOpen();
                }
                else if (ShowingPause)
                {
                    PauseMenuClose();
                }
                else
                {
                    PauseMenuOpen();
                    
                }
            }
            OnSettingsClose();
        }
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OpenSettingsFromPause()
    {
        PauseMenuClose();
        OnSettingsClick();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Destroy(settingsCanvas);
        Destroy(pauseMenu);
        Destroy(gameObject);
    }

    public void PauseMenuClose()
    {
        ShowingPause = false;
        pauseMenu.SetActive(false);
    }

    public void PauseMenuOpen()
    {
        ShowingPause = true;
        pauseMenu.SetActive(true);
    }

    public void OnPlayClick()
    {
        OnSettingsClose();
        AsyncOperation op = SceneManager.LoadSceneAsync("IntroScene", LoadSceneMode.Single);
        op.completed += SceneLoadCallback;
    }

    void SceneLoadCallback(AsyncOperation op)
    {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(1f);
        Scene old = SceneManager.GetActiveScene();
        Scene scene = SceneManager.GetSceneByName("IntroScene");
        SceneManager.SetActiveScene(scene);
        menuCanvas = null;
        menuSrc = null;
        SceneManager.UnloadSceneAsync(old);
    }

    public void OnSettingsClick()
    {
        settingsCanvas.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            menuCanvas.SetActive(false);
        }
        ShowingSettings = true;
        Time.timeScale = 0f;
    }

    public void OnSettingsClose()
    {
        settingsCanvas.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            menuCanvas.SetActive(true);
        }
        ShowingSettings = false;
        Time.timeScale = 1f;
    }
}
