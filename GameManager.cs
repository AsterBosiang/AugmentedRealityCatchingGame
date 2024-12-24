using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("AR References")]
    public Camera arCamera;
    public Transform basket;

    [Header("Game Objects")]
    public GameObject applePrefab;
    
    [Header("UI Elements")]
    public Text scoreText;
    public Text timerText;
    public Text countdownText;
    public Canvas mainCanvas;

    [Header("Game Settings")]
    public float spawnInterval = 1.5f;
    public float gameTime = 25f;
    public float spawnHeight = 800f;

    private int score = 0;
    private bool isGameRunning = false;
    private float countdownTime = 3f;
    private bool isCountingDown = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (scoreText != null) scoreText.text = "Score：0";
        if (timerText != null) timerText.text = "Time：25";
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "3";
        }
        
        StartCountdown();
    }

    private void Update()
    {
        if (isCountingDown)
        {
            UpdateCountdown();
        }
        else if (isGameRunning)
        {
            UpdateGameTimer();
        }
    }

    private void StartCountdown()
    {
        isCountingDown = true;
        countdownTime = 3f;
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }
    }

    private void UpdateCountdown()
    {
        countdownTime -= Time.deltaTime;
        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
        }

        if (countdownTime <= 0)
        {
            isCountingDown = false;
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
            }
            StartGame();
        }
    }

    private void StartGame()
    {
        isGameRunning = true;
        gameTime = 25f;
        InvokeRepeating("SpawnApple", 0f, spawnInterval);
    }

    private void UpdateGameTimer()
    {
        gameTime -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = "Time：" + Mathf.CeilToInt(gameTime).ToString();
        }

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    public void IncreaseScore()
    {
        score += 1;
        if (scoreText != null)
        {
            scoreText.text = "Score：" + score.ToString();
        }
    }

    private void SpawnApple()
    {
        if (!isGameRunning || mainCanvas == null || applePrefab == null) return;

        float randomX = Random.Range(-400f, 400f);
        GameObject apple = Instantiate(applePrefab, mainCanvas.transform);
        RectTransform appleRect = apple.GetComponent<RectTransform>();
        if (appleRect != null)
        {
            appleRect.anchoredPosition = new Vector2(randomX, spawnHeight);
        }
    }

    private void EndGame()
    {
        isGameRunning = false;
        CancelInvoke("SpawnApple");
        
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("GameOverScene");
    }
}
