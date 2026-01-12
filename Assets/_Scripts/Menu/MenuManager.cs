using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject dashboardPanel;
    public GameObject backGround;
    public GameObject menuCanvas;
    public GameObject guildePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1.0f;
    }
    
    public void NewGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OpenDashboard()
    {
        if (dashboardPanel != null)
        {
            dashboardPanel.SetActive(true);
            backGround.SetActive(false);
            menuCanvas.SetActive(false);
        }
    }

    public void CloseDashboard()
    {
        if (dashboardPanel != null)
        {
            dashboardPanel.SetActive(false);
            backGround.SetActive(true);
            menuCanvas.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Authentication");
    }

    public void OpenGuildePanel()
    {
        if (guildePanel != null)
        {
            guildePanel.SetActive(true);
            menuCanvas.SetActive(false);
        }
    }

    public void CloseGuildePanel()
    {
        if (guildePanel != null)
        {
            guildePanel.SetActive(false);
            menuCanvas.SetActive(true);
        }
    }

    public void GoToStart()
    {
        SceneManager.LoadScene("Start");
    }

}
