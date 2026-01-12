using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float worldSpeed;
    public float adjustedWorldSpeed;

    public int critterCounter;
    private ObjectPooler boss1Pool;

    [Header("Level Settings")]
    public int currentLevelIndex; 
    public string completedSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        boss1Pool = GameObject.Find("Boss1Pool").GetComponent<ObjectPooler>();
        critterCounter = 0;
    }


    private void Update()
    {
        adjustedWorldSpeed = worldSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P) || Input.GetButtonDown("Fire3")){
            Pause();
        }

        if (critterCounter > 15)
        {
            critterCounter = 0;
            GameObject boss1 = boss1Pool.GetPooledObject();
            boss1.transform.position = new Vector2(15f, 0f);
            boss1.transform.rotation = Quaternion.Euler(0, 0, -90);
            boss1.SetActive(true);
        }
    }   

    public void Pause()
    {
        if (UiController.Instance.pausePanel.activeSelf == false)
        {
            UiController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.Pause);
        } else
        {
            UiController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.Instance.ExitBoost();
            AudioManager.Instance.PlaySound(AudioManager.Instance.Unpause);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }

    public void SetWorldSpeed(float speed)
    {
        worldSpeed = speed;
    }

    public void WinLevel()
    {
        // 1. Lưu dữ liệu: Mở khóa màn tiếp theo (Level hiện tại + 1)
        // Ví dụ: Đang chơi Level 4 -> Thắng -> Lưu unlocked level là 5
        if (LocalDataManager.Instance != null)
        {
            LocalDataManager.Instance.UpdateLevel(currentLevelIndex + 1);
            Debug.Log("Đã lưu tiến độ: Mở khóa level " + (currentLevelIndex + 1));
        }

        // 2. Chuyển sang màn hình Completed
        if (!string.IsNullOrEmpty(completedSceneName))
        {
            SceneManager.LoadScene(completedSceneName);
        }
        else
        {
            Debug.LogWarning("Chưa nhập tên Scene Completed trong GameManager!");
        }
    }

}
